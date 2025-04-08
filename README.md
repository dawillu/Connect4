# Connect 4 Game

This project is a web-based implementation of the classic Connect 4 game with multiplayer functionality. Players can connect through a WebSocket server and play against each other in real-time.

## Project Structure

The project is organized into two main components:

### Server
- Built with C# using WebSocketSharp for WebSocket communication
- Handles game logic, player matching, and communication
- Located in the [`Server`](Server) directory

### Player (Client)
- Web-based interface built with HTML, CSS, and JavaScript
- Communicates with the server via WebSocket
- Located in the [`Player`](Player) directory

## Features

- Real-time multiplayer gameplay
- In-game chat functionality
- Win detection for horizontal, vertical, and diagonal connections
- Waiting room for player matching
- Customizable player names
- Visual feedback for game actions

## How to Play

1. Launch the server application
2. Open the client webpage in a browser
3. Enter your username and click "Play"
4. Wait for another player to connect
5. Take turns dropping pieces into the grid
6. Connect four pieces horizontally, vertically, or diagonally to win

## Game Rules

- Players take turns dropping colored discs into the grid
- Discs fall to the lowest available position in the selected column
- The first player to form a line of four discs of their color wins
- Lines can be horizontal, vertical, or diagonal

## Setup Instructions

### Running the Server
1. Navigate to the [`Server`](Server) directory
2. Build and run the C# application
3. The server will start on [`ws://127.0.0.1:7890/Play`](Player/Scripts/index_script.js)

### Running the Client
1. Open [`Player/Html/index.html`](Player/Html/index.html) in a web browser
2. Alternatively, host the [`Player`](Player) directory using a web server

## Technologies Used

- C# with WebSocketSharp for the server
- HTML, CSS, and JavaScript for the client interface
- JSON for data exchange between client and server

## Developers

This project was created as a class assignment and won the class competition.

## License

MIT License

Copyright (c) 2025

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.