using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        set { if (instance == null) instance = value; }
    }
    private void Awake()
    {
        Instance = this;
    }

    public float[] HighScores;
    public string[] Names;
    public string[] EncScore;
    public string[] EncName;
    private int MaxHighScores = 10;
    private int key = 129; //129

    public InputField enterName;
    public GameObject UI;
    public GameObject player;
    public GameObject BackButton;
    public GameObject NameEnter;
    public GameObject ApplyButton;
    public Transform playerT;

    public float TimeCounter;
    public bool GameStart = false;
    public bool IsPlaying = false;
    public bool TimerStart = false;
    public bool EndGame = false;
    public bool Drop = false;
    public bool IsNameEntered = false;
    public bool ShowBackButton = false;
    public bool ShowGui = false;
    public bool Cheater = false;
    public bool ShowScores = false;
    public bool SaveAndLoad = true;

    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    
    private void Start()
    {
        RestartGame();
        CreateFilePath();
        LoadScore();
        SaveScore();
     
    }

    void Update()
    {
        if (SaveAndLoad)
        {
            SaveScore();
            LoadScore();
            SaveAndLoad = false;
        }

        CheatChecker();
        //oyun oynanıyorsa
        if (GameStart && IsPlaying)
        {
            if (TimerStart)
            {UpdateTimerUI();}
        }

        if(EndGame)
        {
            SaveScore();
        }
        DeadZone();

        if(!IsNameEntered)
        {
            NameEnter.SetActive(false);
            ApplyButton.SetActive(false);
        }
        else
        {
            NameEnter.SetActive(true);
            ApplyButton.SetActive(true);
        }
    }

    public void UpdateTimerUI()
    {
        TimeCounter += Time.deltaTime;
    }
    //Karakter Yere Düşerse Gerçekleşiyor
 
    public void DeadZone()
    {
        if (player.transform.position.y < -3)
        {
            Drop = true;
        }
        if (Drop && !EndGame)
        {
            playerT.position = new Vector3(0, 2, 0);
            TimeCounter = 0;
            TimerStart = false;
            IsPlaying = true;
            Drop = false;
        }
    }
    //Oyun açılışında oyunu resetliyor
    public void RestartGame()
    {
        TimeCounter = 0;
        GameStart = false;
        IsPlaying = false;
        TimerStart = false;
        EndGame = false;
        Drop = false;
        IsNameEntered = false;
        ShowBackButton = false;
        ShowGui = false;
}
    public void CheatChecker()
    { 
        if(HighScores[0]<8)
        {
            Cheater = true;
        }
    }

    #region SAVE_LOAD_ENC_DEC

    public void CreateFilePath()
    {
        try
        {
            if (!Directory.Exists(path + "/Game"))
            { Directory.CreateDirectory(path + "/Game"); }
            if (!File.Exists(path + "/Game/highscores.txt"))
            {
                File.Create(path + "/Game/highscores.txt");
                for (int i = 0; i < MaxHighScores; i++)
                {
                    HighScores[i] = 999;
                }
                
            }
            if (!File.Exists(path + "/BGameall/scorenames.txt"))
            {
                File.Create(path + "/Game/scorenames.txt");
                for (int i = 0; i < MaxHighScores; i++)
                {
                    Names[i] = "Bot";
                }
            }
        }
        catch
        {
        }
    }

    public void EnterName()
    {
        if (TimeCounter < HighScores[MaxHighScores - 1] && TimeCounter != 0)
        {
            if ((enterName.text != string.Empty) && (IsNameEntered == true))
            {
                Names[MaxHighScores - 1] = enterName.text;
                HighScores[MaxHighScores - 1] = TimeCounter;
                enterName.text = string.Empty;

                float temp;
                string tempName;

                if (TimeCounter < HighScores[MaxHighScores - 1])
                {
                    HighScores[MaxHighScores - 1] = TimeCounter;
                }

                for (int i = MaxHighScores - 1; i > 0; i--)
                {
                    if (HighScores[i] < HighScores[i - 1])
                    {
                        temp = HighScores[i - 1];
                        HighScores[i - 1] = HighScores[i];
                        HighScores[i] = temp;
                        tempName = Names[i - 1];
                        Names[i - 1] = Names[i];
                        Names[i] = tempName;
                    }
                }
                IsNameEntered = false;
            }
        }
    }
    //Key ile ASCII koddaki değeleri ^ (xor)luyor
    public string EncryptDecrypt(string textToEncrypt)
    {
        StringBuilder inSb = new StringBuilder(textToEncrypt);
        StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
        char c;
        for (int i = 0; i < textToEncrypt.Length; i++)
        {
            c = inSb[i];
            c = (char)(c ^ key);
            outSb.Append(c);
        }
        return outSb.ToString();
    }
    
    public void LoadScore()
    {
        FileInfo fileScore = new FileInfo(path + "/Ball/highscores.txt");
        StreamReader readerScore = fileScore.OpenText();
        for (int i = 0; i < MaxHighScores; i++)
        {
            EncScore[i] = readerScore.ReadLine();
            EncScore[i] = EncryptDecrypt(EncScore[i]);
            HighScores[i] = float.Parse(EncScore[i]);
         
        }
        readerScore.Close();

        FileInfo fileNames = new FileInfo(path + "/Ball/scorenames.txt");
        StreamReader readerName = fileNames.OpenText();
        for (int i = 0; i < MaxHighScores; i++)
        {
            EncName[i] = readerName.ReadLine();
            Names[i] = EncryptDecrypt(EncName[i]);
          
        }
        readerName.Close();
    }

    public void SaveScore()
    {  
        /* Encyrption'lı Save*/
        StreamWriter file = new StreamWriter(path + "/Ball/highscores.txt");
        for (int i = 0; i < MaxHighScores; i++)
        {
            file.WriteLine(EncryptDecrypt(HighScores[i].ToString()));
            file.Flush();
        }
        file.Close();

        StreamWriter fileName = new StreamWriter(path + "/Ball/scorenames.txt");
        for (int i = 0; i < MaxHighScores; i++)
        {
            fileName.WriteLine(EncryptDecrypt(Names[i]));
            fileName.Flush();
        }
        fileName.Close();
    }
    #endregion

    #region GUI
    //Skorları ekrana basıyor
    private void OnGUI()
    {
        if (ShowGui)
        {
            UI.SetActive(true);

        }
        if (!ShowGui)
        {
            UI.SetActive(false);
        }
        if (Cheater)
        {
            GUI.Label(new Rect(Screen.width / 2 - 160, Screen.height / 2, Screen.width, Screen.height), "Seni Küçük Hileci!!");
        }

        if ((IsPlaying) && (!Cheater) && !ShowBackButton)
        {
            GUI.skin.label.fontSize = 40;
            GUI.Label(new Rect(Screen.width / 2 - 100, 100, Screen.width, Screen.height), TimeCounter.ToString());
        }
        else if(!IsPlaying && !Cheater && EndGame)
        {
            GUI.skin.label.fontSize = 40;
            GUI.Label(new Rect(Screen.width / 2 - 160, 50, Screen.width, Screen.height), "  1. " + Names[0] + " " + HighScores[0].ToString());
            GUI.Label(new Rect(Screen.width / 2 - 160, 100, Screen.width, Screen.height), "  2. " + Names[1] + " " + HighScores[1].ToString());
            GUI.Label(new Rect(Screen.width / 2 - 160, 150, Screen.width, Screen.height), "  3. " + Names[2] + " " + HighScores[2].ToString());
            GUI.Label(new Rect(Screen.width / 2 - 160, 200, Screen.width, Screen.height), "  4. " + Names[3] + " " + HighScores[3].ToString());
            GUI.Label(new Rect(Screen.width / 2 - 160, 250, Screen.width, Screen.height), "  5. " + Names[4] + " " + HighScores[4].ToString());
            GUI.Label(new Rect(Screen.width / 2 - 160, 300, Screen.width, Screen.height), "  6. " + Names[5] + " " + HighScores[5].ToString());
            GUI.Label(new Rect(Screen.width / 2 - 160, 350, Screen.width, Screen.height), "  7. " + Names[6] + " " + HighScores[6].ToString());
            GUI.Label(new Rect(Screen.width / 2 - 160, 400, Screen.width, Screen.height), "  8. " + Names[7] + " " + HighScores[7].ToString());
            GUI.Label(new Rect(Screen.width / 2 - 160, 450, Screen.width, Screen.height), "  9. " + Names[8] + " " + HighScores[8].ToString());
            GUI.Label(new Rect(Screen.width / 2 - 160, 500, Screen.width, Screen.height), "10. " + Names[9] + " " + HighScores[9].ToString());
            if(!ShowBackButton && EndGame) {
                BackButton.SetActive(false);
                GUI.Label(new Rect(Screen.width / 2 - 220, 560, Screen.width, Screen.height), "Skorunuz " + TimeCounter.ToString());
                GUI.Label(new Rect(Screen.width / 2 - 350, 620, Screen.width, Screen.height), "Yeniden Oynamak İçin Esc'ye Basınız.");
            }
        }
    }
    #endregion  
}
