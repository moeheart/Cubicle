What you need to do:


1. Write Puzzles.json file. 
I don't care how you write this, as long as your own Scene can parse the json file correctly. 
The key of each entry is the room id, and the value is the puzzle
An Example:
{
    "0": {
        "type": "None",
    },
    "1": {
        "type": "Block Builder",
        "size": {
            "x": 3,
            "z": 3
        },
        "maxHeight": 5,
        "height": [[1,0,1],[1,1,0],[0,1,1]]
    }
}

2. You need to get the currentRoomId to know which puzzle to load, do this by using
    int id = DataUtil.GetCurrentRoomId();

3. Once the player completes the puzzle, unlock the current Room. Do this by calling
    DataUtil.UnlockCurrentRoom();

3. In your Scene, the user should be able to go back to the World Scene by pressing "Q" anytime. You can use:
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("World Scene");
		}
	}

4. You can modify Level.json if you need to. Abide by the following format:
{
    "0" : {
        "position": [0, 0 ,0],
        "dimension": [4, 4, 4],
        "color": [1, 0.8, 1, 1],
        "puzzle type": "none"
    },
    "1" : {
        "position": [4,0,0],
        "dimension": [2,2,2],
        "color": [1,1,0,1],
        "puzzle type": "block builder"
    }
}
