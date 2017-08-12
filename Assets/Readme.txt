What you need to do:

Download this repo, import your scene into my project, and create a pull request once ready. 

0. 
The ONLY C# script file you should and need to change is PuzzleType.cs, to register your own puzzles.
(Open the script and you will know what I mean.)

In the prefabs folder, you will find a prefab named "Room", 
in the inspector, you will find a script with the same name ("Room") attached to this prefab.
This script file has a field named "Trigger Prefabs", and it is an array to store the prefabs for the triggers. 
(Each type of puzzle will have a differently looking trigger, 
 so that the player will know what type of puzzle he/she is about to activate.
 Design a trigger for your own puzzle.)
Make sure the "id" for your puzzle in PuzzleType.cs is the same as the index in this array. 
Make sure that your prefab is of suitable size. Add a collider to your prefab. 
Also, attach my TriggerDevice.cs script to your trigger prefab.
You can refer to my Room Builder Trigger prefab to get a sense of how large the prefab should be. 

These should be the ONLY two things that you need to change. 

1. Write Puzzles.json file. 
I don't care how you write this, as long as your own Scene can parse the json file correctly. 
The key of each entry is the room id, and the value specifies the puzzle. 
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
