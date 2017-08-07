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
    int id = DataUtil.getCurrentRoomId();

3. Once the player completes the puzzle, unlock the current Room. Do this by calling
    DataUtil.UnlockCurrentRoom();

3. In your Scene, the user should be able to go back to the World Scene by pressing "Q" anytime. 
