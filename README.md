# Guess-That-Drawing
CS 371 Software Development Project Spring Semester 2023

https://kurt-caves.github.io/Guess-That-Drawing/

# Project Description
Our goal is to create a multiplayer guessing game where one person draws an object, while the other players use a chat box to guess what the object is.

# How to Run
1) Clone the Guess that Drawing repository with the command git clone https://github.com/kurt-caves/Guess-That-Drawing
2) Open the Unity Hub application on your computer. You can download Unity Hub from the official Unity website: 
https://unity.com/download
3) Add the Guess-that-Drawing project to the Unity Hub. In the Unity Hub, click on the "Open" tab. Next, navigate inside of the Guess-That-Drawing folder and click on the "Open" button. Unity should now import and open your project in the editor. 
4) Once you have the project open in Unity, you can compile and run it by clicking on the play button at the top of the Unity Editor.

# changes till 3.29.2023:
Currently, our project contains a simple but functional lobby and chatbox. The lobby allows players to create/join a public server, and the chat box allows for real-time communication within the server. We also started developing a drawing board for players with a variety of tools, including a pen and eraser.

# changes till 4.12.2023:
We added a word bank to our project. At this level before starting the game there are two buttons. One for selecting a word and the other one to set the time for each round. They do not work now, but when you push the "start game" button, the timer will be started.

We began implementing a way to handle player disconnections during the game. Currently, if a client is disconnected from the server, the other players receive a message. If the host is disconnected, the clients are kicked out of the game.

We added a paint bucket feature to our drawing board.

# changes till 5.12.2023:
Currently, our project  functions as a simple drawing and guessing game. We modified the drawing board to be multiplayer, and added several features including a color picker and a pen tool. We also added a mechanism for taking turns, and awarding points for guessing correctly. Additionally, we  updated our program so only the artist is allowed to use the drawing board, and only the guessers are allowed to type in the chat box. 

# Future works:
At this level, the drawer only has access to one word for drawing. You can change the functionality so that the drawer has a chance to select one of three words. Also, you can have a word bank that contains three categories (Easy, Medium, and Hard) and some sub-categories for each one. This would give the option to choose one of these sub-categories.

Another improvement could be adding a waiting room located before starting to draw. This would give some seconds to the drawer to think about how to draw the word.

You can also consider changing the scoring system to a dynamic one. In this system, each player who guesses sooner gets more points, making the game more competitive.

Finally, an option you can add to the game is time settings. The time for each round can be adjusted based on the difficulty of the word chosen by the drawer. For example, if the drawer chooses a hard word, the time could be set to 120 seconds.

