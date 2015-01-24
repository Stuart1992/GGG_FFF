using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Texture FullHeartTexture;
	public Texture EmptyHeartTexture;

	public Texture MenuTitleTexture;
	public Texture LevelTexture;
	public Texture SummaryTexture;
	public Texture TutorialTexture;

	public Font GUIFont;
	public int GUIFontSize;
	private bool guiInitialized=false;

	public LevelData CurrentLevel { get; set; }

	private Transform Player;
	private PlayerStatus pstat;

	private enum GameState
	{
		MenuTitleScreen,
		LevelScreen,
		Running,
		SummaryScreen,
		TutorialScreen
	}
	private GameState state;
	private float lastSwitchTime;

	private System.Random rand;
	private LevelGenerator levelGenerator;
	private LevelInstantiator levelInstantiator;

	void Start () 
	{
		rand = new System.Random();

		GameObject pgo = GameObject.Find ("Player");
		Player = pgo.transform;
		pstat = pgo.GetComponent<PlayerStatus>();

		CurrentLevel = new LevelData(0);
		levelGenerator = new LevelGenerator(rand);
		levelInstantiator = new LevelInstantiator(rand);

		EnterState (GameState.MenuTitleScreen);
	}
	
	void Update () 
	{
		switch(state)
		{
		case GameState.MenuTitleScreen:
			if(Input.GetKeyDown(KeyCode.T))
			{
				EnterState(GameState.TutorialScreen);
			}
			else if(Input.GetMouseButtonDown (0))
			{
				CurrentLevel = levelGenerator.GenerateLevel(1);
				EnterState(GameState.LevelScreen);
			}
			break;

		case GameState.LevelScreen:
			if(Time.time - lastSwitchTime > 1 && (Input.anyKey || Input.GetMouseButton (0)))
			{
				levelInstantiator.InstantiateLevel(CurrentLevel);
				EnterState(GameState.Running);
			}
			break;

		case GameState.Running:
						
			if(CurrentLevel.Over)
			{
				foreach(Transform t in CurrentLevel.Transforms)
				{
					GameObject.Destroy (t.gameObject);
				}
				EnterState (GameState.SummaryScreen);
			}
			break;

		case GameState.SummaryScreen:
			if(Time.time - lastSwitchTime > 1 && (Input.anyKey || Input.GetMouseButton (0)))
			{
				if(CurrentLevel.Victory)
				{
					CurrentLevel = levelGenerator.GenerateLevel(CurrentLevel.Level + 1);
					EnterState(GameState.LevelScreen);
				}
				else
					EnterState(GameState.MenuTitleScreen);
			}
			break;

		case GameState.TutorialScreen:
			if(Input.GetMouseButtonDown(0))
			{
				EnterState(GameState.MenuTitleScreen);
			}
			break;

		}
	}

	void OnGUI()
	{
		if(!guiInitialized)
		{
			GUI.skin.label.font = GUIFont;
			GUI.skin.label.fontSize = GUIFontSize;
			guiInitialized=true;
		}
				
		switch(state)
		{
		case GameState.MenuTitleScreen:
			GUI.DrawTexture(new Rect(5,5, 600,600), MenuTitleTexture); //MenuTitleTexture.width, MenuTitleTexture.width),  MenuTitleTexture);
			break;
		case GameState.LevelScreen:
			GUI.DrawTexture(new Rect(5,5, 600,600),  LevelTexture);
			GUI.TextArea(new Rect(20,20,80,80), "" + CurrentLevel.Level);
			break;
		case GameState.Running:
			// ADD HUD CODE HERE
			ShowHeartHUD();
			break;
		case GameState.SummaryScreen:
			GUI.DrawTexture(new Rect(5,5, 600,600),  SummaryTexture);
			break;
		case GameState.TutorialScreen:
			GUI.DrawTexture(new Rect(5,5, 600,600),  TutorialTexture);
			break;
			
		}
	}

	private void ShowHeartHUD()
	{
		float heartSize = 32f;
		for(int i = 0; i<pstat.MaxHealth; i++)
		{
			Texture tex = (pstat.Health>i) ? FullHeartTexture : EmptyHeartTexture;
			Rect rect = new Rect(1 + i*heartSize*1.2f, 3, heartSize, heartSize);
			GUI.DrawTexture(rect, tex);
			
			//GUI.DrawTexture(new Rect(7*Screen.width/8 + i*heartSize, 7*Screen.width/8, heartSize, heartSize), tex);
			//GUI.DrawTextureWithTexCoords(new Rect(7*Screen.width/8+heartHeight/10 + iconInset, nextBoxYOff+nextBoxHgt/10 + iconInset, mag*nextIconSpace, mag*nextIconSpace ), ntex, new Rect(0,0, 12f/(float)ntex.width,1));
		}
	}
	
	private void EnterState(GameState newState)
	{
		lastSwitchTime = Time.time;
		GameState oldState = state;
		state = newState;

		Debug.Log ("GameController Entering State " + state + " from " + oldState);

		switch(state)
		{
		case GameState.MenuTitleScreen:
			Player.gameObject.SetActive(false);
			break;
		case GameState.LevelScreen:
			Player.gameObject.SetActive(false);
			break;
		case GameState.Running:
			Player.gameObject.SetActive(true);
			Player.position = CurrentLevel.StartPosition;
			break;
		case GameState.SummaryScreen:
			Player.gameObject.SetActive(false);
			break;
		case GameState.TutorialScreen:
			Player.gameObject.SetActive(false);
			break;
			
		}

	}
}
