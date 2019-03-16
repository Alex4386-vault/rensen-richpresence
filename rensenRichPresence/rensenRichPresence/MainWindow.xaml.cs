using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using DiscordRPC.Logging;

namespace rensenRichPresence
{

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public Thread Detector;
        public Thread infoUpdater;
        public Thread discordUpdater;

        public static DiscordRpcClient discord;

        void DiscordInit()
        {
            discord = new DiscordRpcClient(Config.Discord.clientId);
            discord.Initialize();
        }

        public void updateInfo2Discord()
        {
            while (true)
            {
                if (RensenNegotiation.isRensenDetected)
                {
                    if (!RensenNegotiation.AmIDead())
                    {
                        int score = RensenNegotiation.ReadScore();
                        int lifes = RensenNegotiation.ReadLifes();
                        int bombs = RensenNegotiation.ReadBombs();
                        float power = RensenNegotiation.ReadPowerPellets();
                        RensenNegotiation.Difficulty difficulty = RensenNegotiation.ReadDifficulty();
                        if (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ko")
                        {
                            discord.SetPresence(new RichPresence()
                            {
                                Details = "점수: " + score,
                                State = "스펠: " + bombs + ", 잔기: " + lifes + " 난이도: " + RensenNegotiation.Difficulty2String(difficulty)
                                + "\n영력: "+power
                            });
                        }
                        else
                        {
                            discord.SetPresence(new RichPresence()
                            {
                                Details = "Score: " + score,
                                State = "spells: " + bombs + ", lifes:" + lifes + " level:" + RensenNegotiation.Difficulty2String(difficulty)+"\n power:" + power
                            });
                        }

                    }
                    else
                    {
                        int score = RensenNegotiation.ReadScore();
                        RensenNegotiation.Difficulty difficulty = RensenNegotiation.ReadDifficulty();
                        if (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ko")
                        {
                            discord.SetPresence(new RichPresence()
                            {
                                Details = "어, 나 쥬금. 점수:" + score + " @ " + RensenNegotiation.Difficulty2String(difficulty)
                            });
                        }
                        else
                        {
                            discord.SetPresence(new RichPresence()
                            {
                                Details = "Oops, I'm Dead. FinalScore:" + score + " @ " + RensenNegotiation.Difficulty2String(difficulty)
                            });
                        }
                    }
                    for (int i = 1; i <= 10; i++)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        { discordMeter.Value = i * 10; }));
                        Thread.Sleep(Config.Discord.discordSyncMS/10);
                    }
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    { discordMeter.Value = 0; }));
                    
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            if (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ko")
            {
                explainLabel.Text = "이 소프트웨어는 게임데이터를 메모리에서 읽어와\n 디스코드 RichPresence 에 연동해주는\n 프로그램입니다. - 련즐지?";
            }
            DiscordInit();
            Detector = new Thread(new ThreadStart(RensenNegotiation.Detector));
            infoUpdater = new Thread(new ThreadStart(updateInfo));
            discordUpdater = new Thread(new ThreadStart(updateInfo2Discord));

            Detector.Start();
            infoUpdater.Start();
            discordUpdater.Start();
        }

        public void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Detector.Abort();
            infoUpdater.Abort();
            discordUpdater.Abort();
        }

        public void updateInfo()
        {
            while (true)
            {
                if (RensenNegotiation.isRensenDetected)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        rensenDetected.Content = (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ko") ? "성련선을 찾았습니다." : "Detected";
                        rensenMeter.Value = 100;
                        if (!RensenNegotiation.AmIDead())
                        {
                            int score = RensenNegotiation.ReadScore();
                            int lifes = RensenNegotiation.ReadLifes();
                            int bombs = RensenNegotiation.ReadBombs();
                            RensenNegotiation.Difficulty difficulty = RensenNegotiation.ReadDifficulty();
                            String output;
                            if (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ko")
                            {
                                output = "점수:" + score
                                + ",\n 라이프:" + lifes
                                + ", 스펠:" + bombs
                                + ", 영력:" + RensenNegotiation.ReadPowerPellets()
                                + ", 난이도:" + RensenNegotiation.Difficulty2String(difficulty);
                            }
                            else
                            {
                                output = "Score:" + score
                                + ", Lifes:" + lifes
                                + ",\n Spells:" + bombs
                                + ", Power:" + RensenNegotiation.ReadPowerPellets()
                                + ", Difficulty:" + RensenNegotiation.Difficulty2String(difficulty);
                            }
                            scoreLabel.Content = output;

                        }
                        else
                        {
                            scoreLabel.Content = (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ko") ? "유다희" : "YOU DIED";
                        }

                    }));
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        rensenMeter.Value = 0;
                        rensenDetected.Content = (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ko") ? "성련선을 켜세요." : "NOPE";
                    }));
                }
                Thread.Sleep(100);
            }
        }
    }

    public static class RensenNegotiation
    {
        // Exact Same Approach with rensenware, Credit: 0x00000FF, https://github.com/0x00000FF/rensenware-cut/blob/master/Source/RansomNote.cs

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int handleProcess, int localPointerBaseAddr, byte[] localPointerBuffer, int dwSize, ref int NumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int desiredAccessPerm, bool bInheritHandle, int dwProcessId);

        private static IntPtr rensenHandler;

        public static bool isRensenDetected = false;

        public static void Detector()
        {
            while (true)
            {
                Process[] rensenProcesses = Process.GetProcessesByName("th12");

                if (rensenProcesses.Length > 0)
                {
                    rensenHandler = OpenProcess(0x10, false, rensenProcesses.FirstOrDefault().Id);
                    isRensenDetected = true;
                }
                else
                {
                    isRensenDetected = false;
                }

                Thread.Sleep(100);
            }
        }

        public enum Difficulty
        {
            EASY = 0,
            NORMAL = 1,
            HARD = 2,
            LUNATIC = 3,
            ERROR = 99
        }

        public static bool AmIDead()
        {
            if (ReadLifes() < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static String Difficulty2String(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.EASY:
                    return "EASY";
                case Difficulty.NORMAL:
                    return "NORMAL";
                case Difficulty.HARD:
                    return "HARD";
                case Difficulty.LUNATIC:
                    return "LUNATIC";
                case Difficulty.ERROR:
                    return "ERROR";
                default:
                    return "EXTRA";
            }
        }

        public static int ReadLifes()
        {
            int bytesRead = 0;
            byte[] _buffer = new byte[4];

            if (isRensenDetected)
            {
                bool LifeReadTrial = ReadProcessMemory((int)rensenHandler, 0x004B0C98, _buffer, 4, ref bytesRead);
                if (!LifeReadTrial)
                {
                    return Config.Variables.uRFucked;
                }
                return BitConverter.ToInt32(_buffer, 0);
            }
            else
            {
                return Config.Variables.uRFucked;
            }
        }

        public static int ReadBombs()
        {
            int bytesRead = 0;
            byte[] _buffer = new byte[4];

            if (isRensenDetected)
            {
                bool BombReadTrial = ReadProcessMemory((int)rensenHandler, 0x004B0CA0, _buffer, 4, ref bytesRead);
                if (!BombReadTrial)
                {
                    return Config.Variables.uRFucked;
                }

                return BitConverter.ToInt32(_buffer, 0);
            }
            else
            {
                return Config.Variables.uRFucked;
            }
        }

        public static float ReadPowerPellets()
        {
            int bytesRead = 0;
            byte[] _buffer = new byte[4];

            if (isRensenDetected)
            {
                bool PowerPelletsReadTrial = ReadProcessMemory((int)rensenHandler, 0x004B0C48, _buffer, 4, ref bytesRead);
                if (!PowerPelletsReadTrial)
                {
                    return 0;
                }

                return BitConverter.ToInt32(_buffer, 0) / 100;
            }
            else
            {
                return 0;
            }
        }

        public static Difficulty ReadDifficulty()
        {
            int bytesRead = 0;
            byte[] _buffer = new byte[4];

            if (isRensenDetected)
            {
                bool difficultyReadTrial = ReadProcessMemory((int)rensenHandler, 0x004AEBD0, _buffer, 2, ref bytesRead);
                if (!difficultyReadTrial)
                {
                    return Difficulty.ERROR;
                }

                int levelVal = BitConverter.ToInt16(_buffer, 0);

                switch (levelVal)
                {
                    case 0:
                        return Difficulty.EASY;
                    case 1:
                        return Difficulty.NORMAL;
                    case 2:
                        return Difficulty.HARD;
                    case 3:
                        return Difficulty.LUNATIC;
                    default:
                        return Difficulty.ERROR;
                }
            }
            else
            {
                return Difficulty.ERROR;
            }
        }


        public static int ReadScore()
        {
            int bytesRead = 0;
            byte[] _buffer = new byte[4];

            if (isRensenDetected)
            {
                bool scoreReadTrial = ReadProcessMemory((int)rensenHandler, 0x004B0C44, _buffer, 4, ref bytesRead);
                if (!scoreReadTrial)
                {
                    return Config.Variables.errorReadFail;
                }

                return BitConverter.ToInt32(_buffer, 0) * 10;
            }
            else
            {
                return Config.Variables.uRFucked;
            }
        }
    }
}    
