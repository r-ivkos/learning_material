
class enemy { //clase enemigo que gestiona su informacion
  float x; //la posicion X del enemigo
  float y; //la poscion Y del enemigo
  PImage sprite; //su sprite
  float sizeX; //anchura del sprite
  float sizeY; //altura (respecto su tamaño original)
  int lives; //vidas que tiene 
  boolean dead; //si esta mueto (inicialmente false);
  int death_timer = -1; //un contador para mostrar sprite de muerte X secundos
  int type; //tipo del enemigo 
  
  public enemy(PImage sprite, int type, int lives, float sizeX, float sizeY){ //constructor (valores que se teinen que pasar al crear un enemigo)
    this.sprite = sprite;
    x = random(sprite.width, width-sprite.width); //la posicion en X es aleatoria 
    y = -5;
    this.lives = lives;
    this.sizeX = sizeX*sprite.width;  //el tamaño que se le tiene que pasar al constructor es las veces que es mas ancho/alto el sprite (respecto su tamaño original)
    this.sizeY = sizeY*sprite.height;
    this.type = type;
  }
  
  public enemy(PImage sprite, int type, int lives){ //otro constructor por si no hace falta escalar la imagen
    this.sprite = sprite;
    x = random(sprite.width/2, width-sprite.width/2);
    y = -5;
    this.lives = lives;
    sizeX = sprite.width;
    sizeY = sprite.height;
    this.type = type;
  }
  
  public void update(){ //actualiza el estado del enemigo
    if (y >= pY || lives <= 0){ //si esta mas abajo que el jugador o sus vidas llegan a 0, se muere
         if (death_timer ==-1){ //se inicia el temportizador
           death_timer = millis();
         } 
         if (type == 1) image(loadImage("\\enemies\\flowey_d1.png"), x, y, sizeX, sizeY); //dependiendo del tipo de enemigo dibuja un sprite de muerte u otro
         else if (type == 2) image(loadImage("\\enemies\\sans_d1.png"), x, y, sizeX, sizeY);
         if (timer(0.2, death_timer)){ //una vez pasa el tiempo se declara variable "dead" true, finalmente muere y despues se borrara del juego definitivamente
           dead = true;
           if (type == 1) points++; //si el enemigo muere le suman puntos al jugador
           else points+=3; //y le suma 3 puntos por matar el enemeigo dificil
           if (y >= pY){
             pLives--; //ademas si muere porque ha bajado al final, al jugador se le resta una vida
             if (type == 1)points--; //si no lo ha matado el jugador, no le suman puntos
             else points-=3;
           } 
         }
    }
    else {
      image(sprite, x, y, sizeX, sizeY); //si no esta muerto se dibuja y se incrementa su posicion
      y+=enemy_speed;
    }
  }
  public void hit(){ //cuando un proyectil lo golpea, se resta una vida
    lives--;
  }
}
