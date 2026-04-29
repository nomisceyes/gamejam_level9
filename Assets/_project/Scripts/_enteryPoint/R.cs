using UnityEngine;

public static class R
{
    public static class Audio
    {
        public static AudioClip BackgroundMusic;
        public static AudioClip MainMenuMusic;
        public static AudioClip UnitDieSound;
        public static AudioClip MouseClickSound;
        public static AudioClip EvilLaughSound;
        public static AudioClip EvilRoarSound;
        public static AudioClip LoseSound;
        public static AudioClip UnitGrabSound;
    }

    public static void InitAudio()
    {
        Audio.BackgroundMusic = Resources.Load<AudioClip>("Audio/Music/Background");
        Audio.MainMenuMusic = Resources.Load<AudioClip>("Audio/Music/MainMenu");
        Audio.UnitDieSound = Resources.Load<AudioClip>("Audio/SFX/UnitDie");
        Audio.MouseClickSound = Resources.Load<AudioClip>("Audio/SFX/Click");
        Audio.EvilLaughSound = Resources.Load<AudioClip>("Audio/SFX/EvilLaugh");
        Audio.EvilRoarSound = Resources.Load<AudioClip>("Audio/SFX/EvilRoar");
        Audio.LoseSound = Resources.Load<AudioClip>("Audio/SFX/Lose");
        Audio.UnitGrabSound = Resources.Load<AudioClip>("Audio/SFX/GrabUnit");
    }
}