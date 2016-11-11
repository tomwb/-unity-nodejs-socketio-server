// Iniciamos uma nova instância do socket.io passando o objeto http(o servidor http)
var express = require('express');
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);

app.use('/static', express.static(__dirname + '/static'));

app.get('/', function(req, res){
	res.sendFile( __dirname + '/index.html');
});

var rooms = [];
var limit_room = 2;
io.on('connection', function(socket){

	console.log('usuario entrou');

	// defino qual a sala dele
	socket.on('JOIN_ROOM', function( ) {
		// vejo em qual room coloco ele
		var room =  'sala_' + 0;
		if ( Object.keys(rooms).length >= 1 ) {
			var i = 0;
			var checkRoom = false;
			for(var index in rooms) { 
				if ( rooms[index].players.length < limit_room ) {
					room =  'sala_' + i;
					checkRoom = true;
				}
				i++;
			}
			if ( ! checkRoom ) {
				room =  'sala_' + i;
			}

		}
		socket.join( room );

		if ( ! rooms[room] ) {
			rooms[room] = {
				'name' : room,
				'players' : []
			};
		} 
		rooms[room].players.push( {
			'id': socket.id, 
			'ready' : false,
		} );

		io.sockets.in( room ).emit('WAIT_ROOM', {
			'ROOM' : room,
			'TOTAL_PLAYERS' : " " + rooms[room].players.length
		});

		socket.emit('SET_ID', {'ID':socket.id});
		console.log(rooms);
	});

	socket.on('PLAYER_READY', function(){		
		console.log("PLAYER_READY");
		room = getUserRoom(socket);	
		if ( room ) {

			start_game = true;
			var players_ids = [];
			for (var i = 0; i < rooms[room].players.length; i++) {
				players_ids[i] = { ID: rooms[room].players[i].id, POSITION : i };
				console.log(rooms[room].players[i].id);
				// mudo o usuario para ready
				if ( rooms[room].players[i].id != socket.id ) {
					rooms[room].players[i].ready = true;
				}

				if ( ! rooms[room].players[i].ready ) {
					start_game = false;
				}
			}
			if ( start_game ) {
				console.log("COMECOU");
				io.sockets.in(room).emit('START_GAME', { PLAYERS : players_ids });
			}

		}
	});

	socket.on('SET_MOVEMENT', function( data ){		
		room = getUserRoom(socket);	
		if ( room ) {
			console.log(data);
			io.sockets.in(room).emit('GAME_MOVEMENT', {ID: socket.id, POSITION : {x: data.x, y: data.y}});
		}
	});

	socket.on('disconnecting', function( reason ){
		room = getUserRoom(socket);	
		if ( room ) {

			// removo um elemento do array rooms
			var tmpPlayer = [];

			for (var i = 0; i < rooms[room].players.length; i++) {
				if ( rooms[room].players[i].id != socket.id ) {
					tmpPlayer.push( rooms[room].players[i] );
				}
			};
			rooms[room].players = tmpPlayer;

			io.sockets.in( room ).emit('WAIT_ROOM', {
				'ROOM' : room,
				'TOTAL_PLAYERS' : tmpPlayer.length
			});

			//io.sockets.in(room).emit('receiver_chat_message', 'Usuário saiu da sala.');
		}

		console.log("Jogador Saiu");
	});
});

function getUserRoom (socket) {
	for(var index in socket.rooms) { 
		if(socket.rooms[index] != socket.id){
			return socket.rooms[index];
		}
	};
	return null;
}

http.listen(3000, function(){
	console.log('listening on http://localhost:3000');
});