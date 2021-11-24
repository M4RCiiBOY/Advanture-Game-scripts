using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuHandler : MonoBehaviour {

	private int y = 0;
	private int x = 0;

	private bool canMove = false;

	public GameObject optionMenu;
	public GameObject mainMenu;

    private XmlSaveSystem xmlSaver;


    public ImageList[] images;
	public Image selectImage;



	// Use this for initialization
	void Start ()
	{
        //optionMenu.SetActive(false);
        xmlSaver = GameObject.Find("xml").GetComponent<XmlSaveSystem>();
        SelectPos();
	}
	
	// Update is called once per frame
	void Update ()
	{
		AllowToMove();
		PlayerMoveInput();
		PlayerActionInput();
	}

	#region Selection Handel stuff
	/// <summary>
	/// Moves the selected Image
	/// </summary>
	private void SelectPos()
	{

		selectImage.rectTransform.anchoredPosition = images[y].images[x].rectTransform.anchoredPosition;
		selectImage.rectTransform.sizeDelta = new Vector2(images[y].images[x].rectTransform.rect.width, images[y].images[x].rectTransform.rect.height);
	}
	/// <summary>
	/// Handels the movecontroll for the interface
	/// </summary>
	private void AllowToMove()
	{
		if (canMove)
		{
			return;
		}

		if (Mathf.Abs(Input.GetAxis("MenuHorizontal")) < 0.15f && Mathf.Abs(Input.GetAxis("MenuVertical")) < 0.15f)
		{
			canMove = true;
		}
	}
	/// <summary>
	/// Handels the player input for the joystick and the Dpad
	/// </summary>
	private void PlayerMoveInput()
	{
		if (canMove)
		{
			if (Input.GetAxis("MenuVertical") > 0.15f)
			{
				MoveUp();
			}
			else if (Input.GetAxis("MenuVertical") < -0.15f)
			{
				MoveDown();
			}
			else if (Input.GetAxis("MenuHorizontal") > 0.15f)
			{
				MoveRight();
			}
			else if (Input.GetAxis("MenuHorizontal") < -0.15f)
			{
				MoveLeft();
			}

		}
	}
	/// <summary>
	/// To move up in the interface
	/// </summary>
	private void MoveUp()
	{
		if (y > 0)
		{
			y--;
		}

		if (x >= images[y].images.Length)
		{
			x = images[y].images.Length - 1;
		}

		SelectPos();
		canMove = false;
	}
	/// <summary>
	/// To move down in the interface
	/// </summary>
	private void MoveDown()
	{
		if (y < images.Length - 1)
		{
			y++;
		}


		if (x >= images[y].images.Length)
		{
			x = images[y].images.Length - 1;
		}

		SelectPos();
		canMove = false;
	}
	/// <summary>
	/// To move right in the interface
	/// </summary>
	private void MoveRight()
	{
		if (x < images[y].images.Length - 1)
		{
			x++;
		}

		SelectPos();
		canMove = false;
	}
	/// <summary>
	/// To move left in the interface
	/// </summary>
	private void MoveLeft()
	{
		if (x > 0)
		{
			x--;
		}

		SelectPos();
		canMove = false;
	}
	#endregion
	#region Button Handel stuff
	/// <summary>
	/// Handels the player input for buttons
	/// </summary>
	private void PlayerActionInput()
	{
		if (Input.GetButtonDown("MenuButton"))
		{
			if (images[y].images[x].tag=="StartGame")
			{
				// TODO: STUFF
			}

			if (images[y].images[x].tag == "LoadGame")
			{
                SceneManager.LoadScene(1);
			}

			if (images[y].images[x].tag == "Options")
			{
				OpenOptions();
			}

			if (images[y].images[x].tag == "Back")
			{
				CloseOptions();
			}

			if (images[y].images[x].tag == "ExitGame")
			{
				CloseApplication();
			}
		}
	}
	#endregion
	#region Button stuff
	/// <summary>
	/// Opens the optionsMenu in the MainMenu
	/// </summary>
	private void OpenOptions()
	{
		optionMenu.SetActive(true);
		mainMenu.SetActive(false);
		x = 0;
		y = 0;
		SelectPos();
	}
	/// <summary>
	/// Closes the optionsMenu and leads back to the MainMenu
	/// </summary>
	private void CloseOptions()
	{
		mainMenu.SetActive(true);
		optionMenu.SetActive(false);
		x = 0;
		y = 0;
		SelectPos();
	}


	/// <summary>
	/// Closes the Game as build and in the editor
	/// </summary>
	private void CloseApplication()
	{
		if (Application.isEditor)
		{
			// UnityEditor.EditorApplication.isPlaying = false;
		}
		else
		{
			Application.Quit();
		}
	}
	#endregion
}
