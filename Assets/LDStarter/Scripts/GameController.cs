using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public List<Transform> FoodItemPrefabs;
	public List<Transform> KidsList;

	public Texture MenuTitleTexture;
	public Texture LevelTexture;
	public Texture SummaryTexture;
	public Texture TutorialTexture;

	public Texture MomHappy;
	public Texture MomOk;
	public Texture MomGirr;
	public Texture MomAngry;

	public AudioClip jumpSound;
	public AudioClip splat1Sound;
	public AudioClip splat2Sound;
	public AudioClip throw1Sound;
	public AudioClip throw2Sound;
	public AudioClip throw3Sound;
	/*
	public AudioClip hitBroSound1;
	public AudioClip hitBroSound2;
	public AudioClip hitLSSound1;
	public AudioClip hitLSSound2;
	public AudioClip hitBSSound1;
	public AudioClip hitBSSound2;
	*/
	public AudioClip calloutBroSound;
	public AudioClip calloutBigSound;
	public AudioClip calloutLilSound;
	public bool playedCallout;

	public Font GUIFont;
	public int GUIFontSize;
	private bool guiInitialized=false;

	public LevelData CurrentLevel { get; set; }
	public bool IsTimeFrozen=true;

	public int NumFails = 0;
	public int NumProjectiles;
	public int NumProjectilesOrig;
	
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
		CurrentLevel = new LevelData(0);
		levelGenerator = new LevelGenerator(rand,FoodItemPrefabs);
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
			if(true)//Time.time - lastSwitchTime > 1 && (Input.anyKey || Input.GetMouseButton (0)))
			{
				levelInstantiator.InstantiateLevel(CurrentLevel);
				EnterState(GameState.Running);
			}
			break;

		case GameState.Running:
	
			if(Time.time - lastSwitchTime > 0.5f && IsTimeFrozen)
			{
				Time.timeScale *= 0.96f;
				if(Time.timeScale < 0.4f)
				{
				Time.timeScale = 0;
					if(!playedCallout)
					{
					NumProjectilesOrig = NumProjectiles;
					switch(Random.Range (0,3))
					{
					case 0:
						iTween.Stab(gameObject,calloutBroSound,0);
							playedCallout = true;
					break;
					case 1:
						iTween.Stab(gameObject,calloutLilSound,0);
							playedCallout = true;
					break;
					case 2:
						iTween.Stab(gameObject,calloutBigSound,0);
							playedCallout = true;
					break;
					}//end inner switch
					}
				}
			//	Time.timeScale = 0;
			}
			if(NumProjectiles <= 0) //this the condition for when all pies hit the ground
		//	if(Input.GetKeyDown(KeyCode.X) && !IsTimeFrozen)
			{
				CurrentLevel.Over = true;
				CurrentLevel.Victory = true;
				playedCallout = false;
			}
			if(CurrentLevel.Over)
			{
				foreach(Transform t in CurrentLevel.Transforms)
				{
					GameObject.Destroy (t.gameObject);
				}
				EnterState (GameState.SummaryScreen);
			}
			else if(Input.GetKeyDown(KeyCode.Space))
			{
				Debug.Log ("GameController: setting IsTimeFrozen false");
				IsTimeFrozen = false;
				Time.timeScale = 1;
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
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), MenuTitleTexture); //MenuTitleTexture.width, MenuTitleTexture.width),  MenuTitleTexture);
			break;
		case GameState.LevelScreen:
			//GUI.DrawTexture(new Rect(5,5, 600,600),  LevelTexture);
			//GUI.TextArea(new Rect(20,20,80,80), "" + CurrentLevel.Level);
			break;
		case GameState.Running:
			// ADD HUD CODE HERE
		//	ShowHeartHUD();
			if(GUI.Button (new Rect(300,500,100,50),"Click to Run"))
			   {IsTimeFrozen = false;
				Time.timeScale = 1;}

			break;
		case GameState.SummaryScreen:
			if(NumFails <= 0){
				GUI.DrawTexture (new Rect(5,5,300,300), MomHappy);
			}
			else if(NumFails >= NumProjectilesOrig){
				GUI.DrawTexture (new Rect(5,5,300,300), MomAngry);
			}
			else if(((double)NumFails/(double)NumProjectilesOrig) < 0.5){
				GUI.DrawTexture (new Rect(5,5,300,300), MomOk);
			}
			else if(((double)NumFails/(double)NumProjectilesOrig) >= 0.5){
				GUI.DrawTexture (new Rect(5,5,300,300), MomGirr);
			}
			break;
		case GameState.TutorialScreen:
			GUI.DrawTexture(new Rect(5,5, 600,600),  TutorialTexture);
			break;
			
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
	
			break;
		case GameState.LevelScreen:
			break;
		case GameState.Running:
			NumFails = 0;
			ResetKids();
			IsTimeFrozen = true;
			break;
		case GameState.SummaryScreen:
		
			break;
		case GameState.TutorialScreen:
		
			break;
			
		}

	}
	
	private void ResetKids()
	{
		foreach(Transform kid in KidsList)
		{
			JumpInputController jic = kid.gameObject.GetComponent<JumpInputController>();
			if(jic!=null)
			{
				jic.ResetForNewLevel();
			}
		}
	}
}
