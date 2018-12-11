LadrillosYBolitas

Mirar canvas raro ese que ha dicho de proyeccion
Spawner de pelotas
Gamefield  Un gameObject en la escena que si ponemos un prefab en el 0,0, se ponga en la esquina inferior izquierda

//Componentes
BallSpaw, sumidero, deathZone,constructor(creador mapa), detector de input

Tile con metodos virtuales

EN codigo preguntar al screen el tamaño (width y height) sabiendo los pixeles de ancho :(..........
asumir resolucion de 800x1280 (menos que 3/2)

"64" pixeles como sistema de coordenadas

go vacio GameField de tal manera que al poner un bloque en 0,0 se ponga abajo a la izq del tab logico aunque no este centrado

Recortar los sprites para que las casillas sean de 60x60

Componentes:
Hay sumidero y ballSpawner

-Ball. 
	-Start()
	{
		_rb = GETCOMPONENT
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		if (_rb == null)
			Debug.LogError("ME FALTA EL RIGIDBODY")
		#endif
	}

	-Stop(). Busca al Rigidbody y pone la velocidad a 0.
	-StartMoving(Vector3 pos, Vector2 velocity)
	-MoveTo(Vecto3 position,float time,System.Action<Ball> callBack = null)
	{
		StartCoroutine(MoveToCoroutine(position,time));
	}
	-private IEnumerator MoveToCoruitine(Vecto3 position,float time,System.Action<Ball> callBack = null)
	{
		if (callback != null)
		callback(this);
		return yield 0;
	}

-BallSpawnwe:
	-Init(Ball ballPrefab);
	-MoveTo(...)
	-SpawnBalls(uint numBalls, Vector2 dir,Ball ballPrefab)
	{
		Ball newBall = Instantiate(ballprefab).GetComponent<Ball>();
			newBall.StartMoving(transform.position,dir);
			//CADA CUANTOS FIXED UPDATE LE LLAMO
	}

	Siempre tratar de poner componentes especificos

-BallSink.
	-Public Text Label;
	-private uint _numBalls;
	-Reset(uint): 
	-HIDE()/SHOW()//UNA MIERDA DE LINEA


EL BALLSINK SE COLOCA DONDE LA PRIMERA BOLA COLISIONA CON DEATHZONE
SI NO ES LA PRIMERA BOLA, LE DICE A LA BOLA: GO:TO BALLSINK

Cuando la pelota ha llegado informa por un delegate

DeathZone:
	-Init(LevelManager)
	-OnTriggerEnter(Collider2D other)
	{
		Ball b = col.GetComponent<Ball>();
		if (b != null)
			b.Stop();
		
	}

Tile:
	-Init(Action<Tile> callbackDestroyed)
	-uint _pendingTouches,r row, column;
	-Virtual bool MustBeDestroyed {return true; }
	-Virtual bool CanFall {return true; }
	-Virtual bool Touch {return true; }

	-BLOCK: Tile
		OnCollisionEnter
		{
			if (_pendingTouches == 0)
				_levelManager.TileDestroyed(this);
		}

BoardManager:
	Tien el array de tiles
	Tile[,] _board

LevelManager: BallSpawner, BallSkin, AimController, Deathzone, BoardManager
No es singleton