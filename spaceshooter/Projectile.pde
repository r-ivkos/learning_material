
class projectile{ //clase del proyectil que dispara el jugador
  float x; //poscion X
  float y; //posicon Y
  PImage sprite; //imagen del proyectile
  float sizeX;  //ancho
  float sizeY; //alto
  float speed; //velocidad a la que avanza
  boolean destroyed; //si es destruido
  
  public projectile(PImage sprite, float x, float y, float speed, float sizeX, float sizeY){ //constructor
    this.sprite = sprite;
    this.x = x;
    this.y = y;
    this.speed = speed;
    this.sizeX = sizeX*sprite.width;
    this. sizeY = sizeY*sprite.height;
  }
  
  public projectile(PImage sprite, float x, float y, float speed){//otro constructor por si no hace falta escalar la imagen
    this.sprite = sprite;
    this.x = x;
    this.y = y;
    this.speed = speed;
    sizeX = sprite.width;
    sizeY = sprite.height;
  }
  
  public void update(){ //actualiza los proyectiles
    image(sprite, x, y, sizeX, sizeY); //dibuja el proyectil
    y-=speed; // incrementa posicion
    if (y < -5) destroyed = true; //si se ha salido de la pantalla se destruira
    for(int i = 0; i < enemies.size(); i++){ //se repite para cada enemigo que hay en el juego
      enemy enem = enemies.get(i); 
      if (enem.x - enem.sizeX/2 <= x && enem.x+enem.sizeX/2 >= x && 
          enem.y - enem.sizeY/2 <= y && enem.y+enem.sizeY/2 >= y){ 
            enem.hit(); //si el proyectil le da al enemigo llama su funcion "hit" que le restara una vida
            destroyed = true; //y despeus de darle al enemigo, el proyectil se considera destruido
      }         
    }
  }
}
