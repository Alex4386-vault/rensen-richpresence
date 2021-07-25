# rensen-richpresence
Unofficial Discord Rich Presence Implementation of Touhou Project Unidentified Fantastic Object - MIT License Edition

## English
Implementation of Discord Rich Presence for Touhou Project 12: Unidentified Fantastic Object.  
  
### How does it work?
This program uses kernel32.dll's OpenProcess and ReadProcessMemory to read the data in game software itself. (Same programmatical approach with [rensenware](https://github.com/0x00000FF/rensenware-cut))  
This software reads `score`, `level (easy, normal, hard, lunatic)`, `spellcards`, `power`, `life` from the game and forward those data into discord richpresence every 20000ms (you can change that in Config.cs, but discord RichPresence has a rate-limit.)

### Warning!
As declared in **How does it work** Section, It directly accesses memory of game software. which means **It can trigger Anti-Cheat Program** of online games or etc. (e.g. League of Legends (from Riot games) \[I can prove this because i am literally perm-banned by playing LOL while programming this!\])  
  
Please **DO NOT LAUNCH THIS PROGRAM WHILE ANY ANTI-CHEAT PROGRAMS ARE RUNNING**. I **WILL NOT** provide any support for anti-cheat program or online game issues.

### OK, I get it. So how can i use it.
[Screenshots TBD]  
1. Turn the program on
2. Open Discord if you didn't
3. Open Touhou Project 12: Unidentified Fantastic Object as th12.exe
4. Check the correct information is shown at application.
5. Now enjoy your rensen-life

### Help! RichPresence is not updating real-Time!
Since the rate-limit and several limitations of rich-presence, I recommend creating your application and update the client-id.  
You can do those via [Discord Developer Portal](https://discordapp.com/developers).  

### Acknowledgements
This software uses [Lachee's discord RPC Library for C#](https://github.com/Lachee/discord-rpc-csharp), Score reading mechanics were highly inspired by [rensenware](https://github.com/0x00000FF/rensenware-cut)

### LICENSE
MIT License ([HRPL (Hakurei Reimu Public License)](https://github.com/Alex4386/HRPL) is also compatible)

## Korean
당신의 엄청난 련선 라이프를 도와줄 성련선 Discord RichPresence 확장을 소개합니다. 자신이 련선을 얼마나 잘 즐기고 있는지 남들에게 보여주세요!

### 이거 어떻게 작동해요?
동방성련선의 프로그램내부에 있는 메모리를 읽어서 스코어랑 난이도, 남은 스펠카드, 잔기, 영력을 전부 읽은뒤 디코에다가 박아 넣어주는 확장입니다.
(업데이트가 실시간은 아니예요. 디스코드에서 개발자 SDK에 Rate-Limit을 15초에 한번씩 걸어놔서 20초에 한번 씩 밖에 업데이트를 못해요.)  

### 아 근데 15초로 해놨는데도 좀 업데이트가 느린데요?
디스코드 개발자 포털 들어가서 자신이 만든 걸로 클라이언트 아이디를 바꾸세요. [Discord Developer Portal](https://discordapp.com/developers)

### 잠깐!
어떻게 작동하는지 설명을 보시면 알 수 있듯이, 게임 메모리에 직접 접근합니다. 문제는 이것이 **온라인 게임이나 안티-치트 프로그램에 걸릴 수 있습니다**. (예. League of Legends (Riot games) \[이거 짜다가 영정먹었어요. 으으...\])  
  
**절대 안티-치트 프로그램이 켜져 있는 상태에서 이 프로그램을 켜지 마세요.**. 전 **이 이슈에 대해서는 책임 안집니다.**  ~~(당장 저도 영정 먹었다고요)~~

### 사용법
1. 프로그램을 켜세요!
2. 디코를 안켰다면 디코를 켜세요!
3. 성련선을 켜세요!
4. 정보가 프로그램에서 올바르게 읽혀지고 있는지 확인하세요!
5. 즐거운 련선되세요.

### 오픈소스
이 소프트웨어는 [Lachee님이 개발하신 C#용 Discord RPC Library](https://github.com/Lachee/discord-rpc-csharp)을 사용합니다. 스코어와 잔기 읽는 매커니즘은 [Rensenware (련선웨어)](https://github.com/0x00000FF/rensenware-cut) 에서 차용했어요.  

### 라이선스
MIT License로 배포됩니다.  
MIT 라이선스가 본인을 커버하지 않는 경우 [HRPL (하쿠레이레이무 라이선스)](https://github.com/Alex4386/HRPL) 로도 배포중입니다.  

~~힝 OSI, 통과시켜줘...~~
