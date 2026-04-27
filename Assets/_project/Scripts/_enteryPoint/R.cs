using UnityEngine;

public static class R
{
    public static class Audio
    {
        public static AudioClip BackgroundMusic;
        public static AudioClip MainMenuMusic;
        //public static AudioClip RifleShootSound;
        //public static AudioClip RifleReloadSound;
        public static AudioClip MouseClickSound;
    }
    
    public static void InitAudio()
    {
        Audio.BackgroundMusic = Resources.Load<AudioClip>("Audio/Music/Background");
        Audio.MainMenuMusic = Resources.Load<AudioClip>("Audio/Music/MainMenu");
        //Audio.RifleShootSound = Resources.Load<AudioClip>("Audio/SFX/");
        //Audio.RifleReloadSound = Resources.Load<AudioClip>("Audio/SFX/");
        Audio.MouseClickSound = Resources.Load<AudioClip>("Audio/SFX/Click");
    }
}