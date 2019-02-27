
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject Start;
    public Renderer player;
    public Renderer Box;
    public Renderer Box1;
    public Renderer Box2;
    public Renderer Box3;
    public GameObject BackBut;

    public void StartGame()
    {
        GameManager.Instance.GameStart = true;
        GameManager.Instance.IsPlaying = true;
        GameManager.Instance.ShowScores = false;
        Start.SetActive(false);
    }
    public void HighScores()
    {
        GameManager.Instance.LoadScore();
        GameManager.Instance.ShowGui = true;
        GameManager.Instance.IsPlaying = false;
        GameManager.Instance.Cheater = false;
        GameManager.Instance.EndGame = true;
        GameManager.Instance.ShowBackButton = true;
        GameManager.Instance.ShowScores = true;
        GameManager.Instance.IsNameEntered = false;
        
        Start.SetActive(false);
        player.enabled = false;
        Box.enabled = false;
        Box1.enabled = false;
        Box2.enabled = false;
        Box3.enabled = false;
        BackBut.SetActive (true);
    }
    public void Back()
    {
        GameManager.Instance.ShowGui = false;
        GameManager.Instance.IsPlaying = false;
        GameManager.Instance.Cheater = false;
        GameManager.Instance.EndGame = false;
        GameManager.Instance.ShowBackButton = false;

        Start.SetActive(true);
        player.enabled = true;
        Box.enabled = true;
        Box1.enabled = true;
        Box2.enabled = true;
        Box3.enabled = true;
        BackBut.SetActive(false);
    }
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
