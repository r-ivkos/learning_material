

class Ball {
  PVector ballPos; //posicion de la bloa
  PVector ballIncr; //incremento de la bola
  float diam, rad, speed; //diametro, radio y la velocidad de la bola
  boolean destr; //si la bola se tiene que destruir
  color col; //color de la bola
  
  public Ball(float x, float y, float diam){ //constructor
    ballPos = new PVector(x, y);
    ballIncr = new PVector(ball_speed, -ball_speed);
    speed = sqrt(sq(ballIncr.x)+sq(ballIncr.y));
    this.diam = diam;
    rad = diam/2;
    col = color(255, 0, 150);
  }
  
  public Ball(){ //constructor para una pelota nueva  (en caso de pillar un powerUp)
    ballPos = new PVector(game_board.x+game_board.Width/2, game_board.y-15);
    ballIncr = new PVector(ball_speed, -ball_speed);
    speed = sqrt(sq(ballIncr.x)+sq(ballIncr.y));
    diam = balls.get(0).diam;
    rad = diam/2;
    col = color(255, 0, 150);
    ballIncr.x = random(-speed+0.05*speed, speed-0.05*speed); //escoge direccion aleatoria
 
    ballIncr.y = -sqrt(sq(speed)-sq(ballIncr.x));
  }
  
 
  void update(){
    fill(col);
    noStroke();
    if(game_started){ //si el juego ha empezado, incrementa la poscion de la bola
      ellipse(ballPos.x, ballPos.y, diam, diam);
      ballPos.add(ballIncr);    
    }
    else { //si no, imprime la bola pegada a la raqueta
      ballPos.x = game_board.x+game_board.Width/2;
      ellipse(ballPos.x, ballPos.y, diam, diam);
    }
    //rebotes con la pantalla
    if(ballPos.x-rad <= 0) ballIncr.x = abs(ballIncr.x);
    if(ballPos.y-rad <= 0) ballIncr.y = abs(ballIncr.y);
    if (ballPos.x+rad >= width) ballIncr.x = -abs(ballIncr.x);
    if (ballPos.y-rad >= height) {//si cae abajo
      if(balls.size() == 1){ //si solo hay una pelota, se hace el caso de perder una vida
        objs.clear(); //borra todos los objetos que pueden estar cayendo
        shield_timer = -1;
        game_started = false; //se fija la bola
        ballPos.y = game_board.y-rad; //se cambia la posicion de la bola
        col = color(255, 0, 150);
        pLives--; //el jugador pierde una vida
        if (pLives <= 0){ //si el jugador ha perdido todas sus vidas, se muestra la pantalla gameover
          game_window = 4;
          game_pause = true;
        }
      }
     else {
       destr = true;
     }
    }
    //rebote con la pala
    if(ballPos.y/*-rad/4*/ > game_board.y && ballPos.y - rad < game_board.y+game_board.Height){
      if(ballPos.x+rad > game_board.x && ballPos.x+rad < game_board.x+game_board.Width/2){
        ballIncr.x = -speed+0.1*speed;
        ballIncr.y = -sqrt(sq(speed)-sq(ballIncr.x));
      } 
      else if (ballPos.x-rad < game_board.x+game_board.Width && ballPos.x-rad > game_board.x+game_board.Width/2){
        ballIncr.x = speed-0.1*speed;
        ballIncr.y = -sqrt(sq(speed)-sq(ballIncr.x));
      }
    }//en caso del escudo, si la bola no toca la pala, rebota igualmente con eñ escudo
    else if (shield_timer != -1 && ballPos.y + rad > game_board.y){
      ballIncr.y = -abs(ballIncr.y);
    }
    else if (ballPos.y+rad > game_board.y-5 && ballPos.y+rad < game_board.y+game_board.Height/2 && ballPos.x+rad > game_board.x && ballPos.x-rad < game_board.x+game_board.Width){
      float col_pos = map(ballPos.x, game_board.x, game_board.x+game_board.Width, -speed, speed);
      if (col_pos < -speed+0.5) col_pos = -speed+0.5;
      if (col_pos > speed-0.5) col_pos = speed-0.5;
      //ballIncr.set(col_pos, -sqrt(sq(speed)-sq(col_pos)));
      ballIncr.x = col_pos;
      ballIncr.y = -sqrt(sq(speed)-sq(ballIncr.x));
    }
    
   
    for(Rectangle brick : bricks){ //foreach que se repite para cada bloque de la lista
        if(brick.CircleCollision(ballPos.x,ballPos.y,rad)){ //si la bola colisiona con el bloque
         int colX = brick.colX; //se guarda la posicion relativa de choque (indica a que lado del bloque ha chocado la bola)
         int colY = brick.colY; //colX:  0 - izquierda, 1 - derecha, -1 en medio // colY: 0 - arriba, 1 - abajo, -1 en medio
          if (colX == -1){
            switch(colY){
             case 0: 
               ballIncr.y = -abs(ballIncr.y);
               break;
             case 1:
               ballIncr.y = abs(ballIncr.y);
               break;
            }
          }
          else if (colY == -1){
            switch(colX){
             case 0: 
               ballIncr.x = -abs(ballIncr.x);
               break;
             case 1:
               ballIncr.x = abs(ballIncr.x);
               break;
            }
          }
          if (colX == -1 && colY == -1) ballIncr.set(-ballIncr.x, - ballIncr.y); //Indeterminado
          //choques con esquinas
          if (colX == 0 && colY == 0) { // Arriba-Izquirda
            if (ballIncr.x > 0)  ballIncr.x = -abs(ballIncr.x);
            if (ballIncr.y > 0) ballIncr.y = -abs(ballIncr.y);
          }
          if (colX == 1 && colY == 0) { //Arriba-Derecha
            if (ballIncr.x < 0)  ballIncr.x = abs(ballIncr.x);
            if (ballIncr.y > 0) ballIncr.y = -abs(ballIncr.y);
          }
          if (colX == 0 && colY == 1) { //Abajo-Izquierda
            if (ballIncr.x > 0)  ballIncr.x = -abs(ballIncr.x);
            if (ballIncr.y < 0) ballIncr.y = abs(ballIncr.y);
          }
          if (colX == 1 && colY == 1) { //Abajo-Derecha
            if (ballIncr.x < 0)  ballIncr.x = abs(ballIncr.x);
            if (ballIncr.y < 0) ballIncr.y = abs(ballIncr.y);
          }
          
          col = color(red(brick.c), green(brick.c), blue(brick.c)); //el color de la bola sera el del brick chocado
          brick.hit(); //toca el brick
      }
    }
  }
}
