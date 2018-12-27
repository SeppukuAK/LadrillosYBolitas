LadrillosYBolitas

Mencionó un Canvas raro de proyección

Gamefield: Un gameObject en la escena que si ponemos un prefab en el 0,0, se ponga en la esquina inferior izquierda

-------- ASPECT RATIO ---------
EN codigo preguntar al screen el tamaño (width y height) sabiendo los pixeles de ancho 

asumir resolucion de 800x1280 (menos que 3/2)
En moviles es un circo el aspect Ratio

Hay que tener sprites diferentes para cada tipo DPI

Relacion de aspecto del tablero:
11,25 ancho
14 alto
-------- ASPECT RATIO ---------

-------- SPRITES ---------

Todos los sprites:
"64" pixeles como sistema de coordenadas

Recortar los sprites para que las casillas sean de 60x60
-------- SPRITES ---------

//Componentes

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

BoardManager o constructor(creador mapa):
	Tien el array de tiles
	Tile[,] _board

LevelManager: BallSpawner, BallSink, AimController o detector de input, Deathzone, BoardManager
No es singleton

Un delegado es una funcion que se pasa como parametro

---Cosas de guardado-----
Persistencia. Hay que guardar el progreso del jugador
Serializacion.
c# tiene serializacion nativa.

unity tiene la propia para json
jsonutilities.
Le enchufas una clase y te la convierte
Hay que marcar las clases como serializables.
No funciona muy bien. Sobre todo con diccionararios.
Nos da una cadena que podremos guardar en una cadena de un fichero.

Esta la operacion inversa:
le das una cadena y te devuelve el progreso del jugador.

Persistencia

En Unity hay PlayerPrefs. 
Es un diccionario en el que guardas cosas: Volumen = 5
No le gusta.

Otra opcion son los ficheros

La opcion es en la nube""

para guardarlo lo guardamos en el sistema

WINDOWS: regedit.exe

primer nivel de proteccion: Añadir una Hash: Es una funcion probabilisticamene inyectiva.
Una hash sirve para resumir

---Cosas de guardado-----

---------------18-12-2018---------------

-------ANUNCIOS-------------
--> Examen : en lab y habra parte de teoria y parte de practica que sera sobre nuestras prácticas

///  Adds: como funcionan los anuncios?  ////

El modelo que se usa es que hay intermediarios que se ponen en contacto con anunciantes
Lo normal es que el que tiene la pag /juego se conecta con un proveedor de publicidad(intermediario) y elige el anuncio y a su vez Los anunciantes hablan con el intermediario 
y ofrecen sus anuncios.

En Internet se tiene un perfil amplio de los Usuarios
Google adds, double click (comprado por Google)

Los intermediarios ponen una cantidad de ingresos a la que tiene que llegar para monetizar

IAP/IAB: in application purchases

Cuando tenemos acceso a lo que tiene Unity para los ads(??? ---> Unity ads. hay que tener una organización en Unity Teams(?, se asocia un id de proyecto y se pueden activar adds. Tenemos que descargamos el plugin para adds
 
permite poner un banner en la parte de abajo y a pantalla completa (Un poco kk)

abrimos la parte de los servicios de unity -> en assets, tenemos un id de unity en los servidores de unity aaa

id.unity.com tiene el dashboard con el panel de control de nuestro proyecto

Se puede ver de forma separada las ganancias en android e IOS

En nuestra practica se verán los placeholders de los anuncios, pero nada más

Si se hace dede el editor unity manda un mensaje(?)

chart boost(?) mas funcionalidades que Unity (Oji)

**hay que subir el juego a la plataforma, hacerlo para android e ios (esto no es para la practiqui)

Cuando se crea una cuenta de desarrollador, cuando se sube el apk, hay que firmarlo (firma digital)
Diferencia entre http y https, tienes un certificado digital()


Dos formas de encriptar -> encriptacion simetrica : encriptacion en la que el emisor y receptor tienen un secreto compartido(password) : cifrado cesar
el receptor usa la contraseña para leer el mensaje

En Amazon se usa encriptacion simetrica, tiene un problema: se necesita un secreto compartido // para comunicacion con gente random no sirve - se necesita un canal seguro donde se haya podido compartir la info

encriptacion asimetrica: se calculan a la vez dos claves, una para encriptar y la otra para desencriptar, con una sola clave no se puede

si quiero mandar algo te doy el candadoy me guardo la llave(??????) al mundo le digo mi clave de encriptacion, el resto lo encripta y yo lo desencripto (es MUUY LENTAAAAAAAA)

entidad certificadora es 


-------ANUNCIOS-------------


---------------18-12-2018---------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acelerometro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 ac = Input.acceleration;
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalirYVolver : MonoBehaviour {

	// Use this for initialization
	void Start () {

#if !UNITY_ANDROID
        Destroy(this.gameObject);
#endif

    }

#if UNITY_ANDROID

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); //Se carga la aplicación :(
                                //Hay que ser amables :(
        }
    }
#endif

}


ESTO ES DEL PRIMER DIA CON UNITY
-----------COSAS RARAS CON SOLO SDK--------------------

Tiene el emulador abierto y puede lanzarlo ahi. No se como

DOnde esta la sdk instalada:
tools/bin/avdmanager.bat list avd

ADB. Punto de dupuración de android. Para depurar en Unity android

platform-tools/ adb devices
Muestra los dispositivos coenctados

adb install e:cubo.apk
Cuando se hace una build para android en Unity. Se tiene en cuenta el hardware

Lo que hace pepa para hacer build rapida:
se crea un .cat
set PATH=%PATH ERES UN GILIPOLLAS SUBNORMAL TE PARTIA LA CARA

abre cmd:
-android
-magia

adb install cubo.apk
adb -e -> Emulador
adb -d -> Movil
adb -s "Numero de serie" -> Cuando hay muchas cosas conectadas
adb install -r cubo.apk -> Si existe lo borra y crea el nuevo

abd shell -> Cosas araras de meterse por los archivos del dispositivo

Para que existan los debug.log. Unity los mapea a .log de Android

-----------COSAS RARAS CON SOLO SDK--------------------

