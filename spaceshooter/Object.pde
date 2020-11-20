class object{ //clase objeto
  float x; //posicion X del objeto
  float y; //posicion Y del objeto
  float sizeX; //anchura del sprite 
  float sizeY; //altura del sprite
  int type; //tipo del objeto
  PImage sprite; //sprite que tiene
  boolean player_detected; //si el jugador ha cogido el objeto
  boolean destroyed; //si es destruido
  
  public object(PImage sprite, int type, float x, float y){ //constructro del objeto 
    this.type = type;
    this.sprite = sprite;
    this.x = x;
    this.y = y;
    sizeX = sprite.width;
    sizeY = sprite.height;
  }
   public object(PImage sprite, int type, float x, float y, float sizeX, float sizeY){ //otro constructor por si hace falta escalar la imagen
    this.type = type;
    this.sprite = sprite;
    this.x = x;
    this.y = y;
    this.sizeX = sprite.width*sizeX;
    this.sizeY = sprite.height*sizeY;
  }
  
  public void update(){ //actualiza la informacion del objeto
    image(sprite, x, y, sizeX, sizeY); //lo dibuja
    y+=object_speed; //incrementa su poscion
    if (y > height+5) destroyed = true; //si se ha salido de la pantalla, se destruira
    if (x >= pX-(ship.width*2)/2 && x <= pX+(ship.width*2)/2 &&
        y >= pY-(ship.width*2)/2 && x <= pY+(ship.width*2)/2 ){
      player_detected = true; //si el jugador lo ha cogido, hara el efecto que tiene que hacer 
    }     
  }
  
}
