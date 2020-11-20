import processing.core.*; 
import processing.data.*; 
import processing.event.*; 
import processing.opengl.*; 

import java.util.List; 
import java.util.ArrayList; 

import java.util.HashMap; 
import java.util.ArrayList; 
import java.io.File; 
import java.io.BufferedReader; 
import java.io.PrintWriter; 
import java.io.InputStream; 
import java.io.OutputStream; 
import java.io.IOException; 

public class brickbreaker extends PApplet {




int pLives; //vidas del jugador
boolean game_started; //en true, bloquea el moviemineto de la bola
boolean game_pause; //juego en pausa
int game_window; //la ventana actual del programa
int game_lvl; //nivel del juego

int shield_timer, shield_timer_ef, shield_ef; //temporizador de escudo / temp. del efecto del escudo / estado del efecto del escudo
float object_speed = 5; //velocidad de caida de objetos
float board_speed; //velocidad de la raqueta
float ball_speed; //velocidad de la pelota
float bsizeX, bsizeY; // posicion de la raqueta
Board game_board; // la raqueta


List<Rectangle> bricks = new ArrayList<Rectangle>(); //lista de bricks
List<Ball> balls = new ArrayList<Ball>(); //lista del pelotas;
List<object> objs = new ArrayList<object>(); //lista de objetos de mejora
PFont font[] = new PFont[2]; //una array de fuentes para Arial y Arial negrita


public void setup(){
  
  pLives = 3;
  game_window = 1;
  game_lvl = 1;
  game_pause = true;
  shield_timer = -1;
  shield_timer_ef = -1;
  shield_ef = 1;
  board_speed = 20;
  ball_speed = 8;
  bsizeX =250;
  bsizeY = 30;
  game_board = new Board(width/2-bsizeX/2, height-80, bsizeX, bsizeY, color(0, 255, 0));
  font[0] = createFont("Arial", 1); //indico las fuentes
  font[1] = createFont("Arial Bold", 1);
  float brix = 5; //margen X del borde de la pantalla
  float briy = 50; //margen Y del borde 
  float briWidth = 105, briHeight = 50; //tamaño de cada bloque
  float xSeparation = 7, ySeparation = 5; //separacion entre bloques
  float bColumns = 8; //cantidad de columnas de bloques
  float bRows = 5; //cantidad de filas
  for(int i = 0; i < bColumns; i++){ //dibuja los bloques siguiendo un patrón
    for (int j = 1; j <= bRows; j++){
      int bc = colorSelector(); //elige un color aleatorio para cada bloque
      if (j%2 != 0) bricks.add(new Rectangle(brix+(briWidth+xSeparation)*i, (briy+ySeparation)*j, briWidth, briHeight, bc));
      
      else if (i <= bColumns-2)bricks.add(new Rectangle(brix+briWidth/2+(briWidth+xSeparation)*(i), (briy+ySeparation)*j, briWidth, briHeight, bc, 3));
    }
  }
  balls.add(new Ball(game_board.x+game_board.Width/2, game_board.y-15, 30));        
}


public void draw(){
  background(0);
  if (!game_pause){
    game_board.drawRect(); //dibujar la raqueta
    bricks_update(); //actualización del estado de cada bloque
    balls_movement();
    objects_update();
    if (!game_started){ //si el movimiento de la bola esta bloqueado, se imprime un texto
      fill(150);
      textFont(font[1], 50);
      textAlign(CENTER, CENTER);
      text("Pulsa ESPACIO", width/2, 600);
    }
   fill(255, 150, 0);
   textFont(font[0], 30);
   textAlign(LEFT, UP);
   text("Vidas: " + pLives, 5, 30); //imprimr la cantidad de vidas
  }
  else if (game_window != 3) { //si el juego esta en pausa y no esta en la pantalla del juego
    window_manager();  //imprime la pantalla correspondiente
  }
  
  if(balls.size() == 0){ //(por si acaso) en caso de no haber pelotas, crea una y la bloquea en la pala
    balls.add(new Ball(game_board.x+game_board.Width/2, game_board.y-15, 30));
    game_pause = false;
  }
  
  //dibujar el escudo durante 10 segundos y cada 0,03 seg. cambiar el color de este (para hacer un efecto de parpadeo)
  if(shield_timer!= -1 && millis()-shield_timer <= 10000){
    if(millis()-shield_timer_ef >= 30){ 
     if(shield_ef == 1) {
       stroke(200);
       shield_ef = 2;
     }
      else{ 
        stroke(255);
        shield_ef = 1;
      }
      shield_timer_ef = millis();
    }
    strokeWeight(10);
    line(0, game_board.y, width, game_board.y);
  }
  else {
    shield_timer = -1;
    shield_timer_ef = -1;
  }
  
}

public void keyPressed(){
  if (!game_pause) game_board.move(); //si se pulsa una tecla el juego no esta pausado, intentará mover la pala
  if (key == ' ' && !game_started){ //si se pulsa espacio y el juego aun no ha epezado(la bola bloqueada)
      game_started = true; //inicializa el juego
      //escoge direccion aleatoria para cada pelota en funcion del modulo de su velocidad
      for(Ball b: balls){
        b.ballIncr.x = random(-b.speed+0.1f*b.speed, b.speed-0.1f*b.speed);
        b.ballIncr.y = -(sqrt(sq(b.speed)-sq(b.ballIncr.x)));
      }
  }
  if (game_window == 3){ //si la pantalla del juego es la del propio juego
    if (key == 'p' || key == 'P' || keyCode == TAB){ 
      if (!game_pause){ //presionando P o tabulador pausara el juego e imprimira un texto
        game_pause = true;
        fill(0, 0, 0, 150);
        rect(0, 0, width, height);
        fill(155);
        textFont(font[1], 50);
        textAlign(CENTER, CENTER);
        text("PAUSA", width/2, height/2);
        noLoop();
      }
     else { //si el juego ya esta pausado se quita la pausa
       game_pause = false;
       loop();
     }
    }
  }
}

public void balls_movement(){
  int destr = -1;
  for(int i = 0; i < balls.size(); i++){
    balls.get(i).update();
    if(balls.get(i).destr) destr = i;
  }
  if (destr != -1) balls.remove(destr);
}


public void bricks_update(){ //imprimir y borrar bloques
  int toDestroy = -1;
  for(int i = 0; i < bricks.size(); i++){
    Rectangle rect = bricks.get(i);
    rect.drawRect(); //dibuja cada bloque
    if (rect.destr) toDestroy = i; //apunta en que posicion de la lista esta el bloque que se ha destruido
  }
  if (toDestroy != -1) bricks.remove(toDestroy); //remueve de la lista, y por tanto del juego, el bloque destruido
    
  if (bricks.size() == 0){ //si en la lista de bloques no queda ni uno, el jugador ha pasado el juego
    game_pause = true;
    shield_timer = -1;
    game_window = 5; //la pantalla se cambia a la de la felicitaciin
  }
 else if (game_lvl == 3){ //si es el 3r nivel imprime la vida del unico bloque que hay en este
   textAlign(CENTER, CENTER);
   textFont(font[1], 100);
   Rectangle boss = bricks.get(0);
   fill(map(boss.lives, 0, boss.inlives, 255, 0));
   text(boss.lives, boss.x+boss.Width/2, boss.y+boss.Height/2);
 }
}

public void objects_update(){ //imprimir y activar efecto de objetos
  int toDestroy = -1; //indice del objeto destruido (caido abajo)
  int pdet = -1; //indice del objeto que ha cogido el jugador
  for(int i = 0; i < objs.size(); i++){
    object obj = objs.get(i);
    obj.update();
    if(obj.destroyed) toDestroy = i;
    if(obj.player_detected) pdet = i;
  }
  if(toDestroy != -1) objs.remove(toDestroy);
  if(pdet != -1){
    switch(objs.get(pdet).type){ //aplica el efecto segun el tipo de mejora
      case 1:
        balls.add(new Ball());
        balls.add(new Ball());
        balls.add(new Ball());
        break; 
        
     case 2:
        game_board.Width+=15;
        break;
        
      case 3:
        game_board.Width-=15;
        break;
        
      case 4:
        shield_timer = millis();
        shield_timer_ef = millis();
        break;
    }
    objs.remove(pdet);
  }
}



public void window_manager(){ //imprime la pantalla correspondiente
  int col = color(255, 150, 0);
  int selCol = color (255, 255, 0);
  switch(game_window){
    case 1:
      textFont(font[1], 60); //establecer la fuente y el tamaño del texto
      textAlign(CENTER, CENTER); //puntos X Y del texto son el centro del texto
      fill(255, 255, 100); //color del texto
      text("BRICK BREAKER", width/2, 100);
      textFont(font[1], 40);
      button b1 = new button(width/2, 300, "JUGAR", col, selCol); //crear los 3 botones en el menu
      button b2 = new button(width/2, 400, "AYUDA", col, selCol);
      button b3 = new button(width/2, 500, "SALIR", col, selCol);
      b1.update(); 
      b2.update();
      b3.update();
      if (b1.selected && mousePressed){ //si el boton esta pulsado, se inicia el juego
        game_window = 3;
        game_pause = false;
      }
      if (b2.selected && mousePressed){ //se pasa a otra ventana 
        game_window = 2;
      }
      if (b3.selected && mousePressed){ //salir del juego
        exit();
      }
      break;
    case 2:
      textAlign(CENTER, CENTER);
      textFont(font[0], 40);
      fill(100);
      text("Tu objetivo es romper bloques con la pelota, \nla cual tendras que hacer rebotar con la pala\nque controlas. Para controlar la pala pulsa \n\"A\"\\\"D\" o flechas (izquierda/derecha).\n"+
           "Si dejas caer abajo la pelota pierdes una vida,\npierdes el juego si pierdes las 3 vidas que \ntienes.\n" + 
           "Finalmente, si rompes todos los bloques \nque hay, ganas el juego.\nSi estas cansado, siempre podras \npausar el juego pulsando \"P\"", width/2, height/2-100);
      textFont(font[1], 40);
      button b4 = new button(width-200, height-100, "VOLVER", col, selCol);
      b4.update();
      if(b4.selected && mousePressed){
        game_window = 1;
      }
      break;
   case 4: 
     textAlign(CENTER, CENTER);
     textFont(font[1], 60);
     fill(156, 40, 0);
     text("GAME OVER", width/2, height/4);
     textFont(font[0], 30);
     fill(150);
     if (game_lvl == 3) text("Inesperado, ¿verdad?", width/2, 340);
     else if (bricks.size() <= 8 && bricks.size() > 1) text("Te han faltado solo " + bricks.size() + " bloques por destruir", width/2, 340);
     else if (bricks.size() == 1) text("¡Vaya! Solo te quedaba un bloque...", width/2, 340);
     else if (bricks.size() > 8) text("Han quedado " + bricks.size() + " bloques por destruir", width/2, 340);
     textFont(font[1], 40);
     button b5 = new button(width/2, 450, "INTENTAR DE NUEVO", col ,selCol);
     button b6 = new button(width/2, 520, "SALIR", col, selCol);
     b5.update();
     b6.update();
     if (b5.selected && mousePressed) restart();
     if (b6.selected && mousePressed) exit();
     break;
     
   case 5:
     if(game_lvl < 3){ //si no es el ultimo nivel, muestra la pantalla de final del nivel
       textAlign(CENTER, CENTER);
       textFont(font[1], 60);
       fill(0, 240, 50);
       text("Nivel " + game_lvl + " completado!", width/2, height/4);
       textFont(font[0], 30);
       fill(150);
       text("Felicidades, has pasado este nivel. \nPuedes seguir jugando o salir.\n¡Ten en cuenta que si sales, pierdes todo tu progreso!", width/2, 340);
       textFont(font[1], 40);
       button b7 = new button(width/2, 450, "SIGUIENTE NIVEL", col ,selCol);
       button b8 = new button(width/2, 520, "SALIR", col, selCol);
       b7.update();
       b8.update();
       if (b7.selected && mousePressed){
         game_lvl++;
         restart();
       }
       if (b8.selected && mousePressed) exit();
     }
     else game_window = 6; //si es el ultimo, se cambia a la pantalla de victoria
     break;
   case 6:
     textAlign(CENTER, CENTER);
     textFont(font[1], 60);
     fill(0, 240, 50);
     text("Victoria", width/2, height/4);
     textFont(font[0], 30);
     fill(150);
     text("De momento no hay mas niveles, asi que\n¡felicidades, te has pasado el juego!", width/2, 340);
     textFont(font[1], 40);
     button b9 = new button(width/2, 520, "SALIR", col, selCol);
     b9.update();
     if (b9.selected && mousePressed) exit();
     break;
  }
}


public void restart(){ //reinicia el juego
  bricks.clear();
  balls.clear();
  objs.clear();
  pLives = 3;
  game_window = 3;
  game_pause = false;
  game_started = false;
  board_speed = 20;
  bsizeX =250;
  bsizeY = 30;
  game_board = new Board(width/2-bsizeX/2, height-80, bsizeX, bsizeY, color(0, 255, 0));
  
  float brix;
  float briy;
  float briWidth, xSeparation;
  float briHeight, ySeparation;
  float bColumns;
  float bRows;
  int bc;
  switch(game_lvl){ //dependiendo del nivel cambia el patron de la pared de bloques
    case 1:
      brix = 5;
      briy = 50;
      briWidth = 105; 
      briHeight = 50; 
      xSeparation = 7;
      ySeparation = 5;
      bColumns = 8;
      bRows = 5;
      for(int i = 0; i < bColumns; i++){
        for (int j = 1; j <= bRows; j++){
          bc = colorSelector();
          if (j%2 != 0) bricks.add(new Rectangle(brix+(briWidth+xSeparation)*i, (briy+ySeparation)*j, briWidth, briHeight, bc));
          
          else if (i <= bColumns-2)bricks.add(new Rectangle(brix+briWidth/2+(briWidth+xSeparation)*(i), (briy+ySeparation)*j, briWidth, briHeight, bc, 3));
        }
      }  
      break;
    case 2: 
      brix = 5;
      briy = 50;
      briWidth = 105; 
      briHeight = 50; 
      xSeparation = 7;
      ySeparation = 5;
      bColumns = 8;
      bRows = 5;
       for(int i = 0; i < bColumns; i++){
        for (int j = 1; j <= bRows; j++){
          bc = colorSelector();
          if ((i%2 == 0 && j%2 != 0) || (i%2 != 0 && j%2 == 0)) bricks.add(new Rectangle(brix+(briWidth+xSeparation)*i, (briy+ySeparation)*j, briWidth, briHeight, bc, 3));
          else bricks.add(new Rectangle(brix+(briWidth+xSeparation)*i, (briy+ySeparation)*j, briWidth, briHeight, bc));
        }
      }
      break;
    case 3:
     bc = colorSelector();
     bricks.add(new Rectangle(width/2-200, 150, 400, 200, bc, 30));
  }
  
  balls.add(new Ball(game_board.x+game_board.Width/2, game_board.y-15, 30));
}

public int colorSelector(){ //selector aleatorio de colores.
  int rand = (int)random(1, 11);
  switch(rand){
    case 1:
      return color(230, 44, 4); //retorna un color
    case 2:
      return color(55, 216, 8);
    case 3:
      return color(14, 243, 232);
    case 4:
      return color(14, 49, 243);
    case 5:
      return color(243, 190, 13);
    case 6:
      return color(235,13,243);
    case 7:
      return color(238, 239, 5);
    case 8:
      return color(69, 222, 166);
    case 9:
      return color(240, 116, 199);
    case 10:
      return color(123, 255, 47);
  }
  return color(0);
}


class Ball {
  PVector ballPos; //posicion de la bloa
  PVector ballIncr; //incremento de la bola
  float diam, rad, speed; //diametro, radio y la velocidad de la bola
  boolean destr; //si la bola se tiene que destruir
  int col; //color de la bola
  
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
    ballIncr.x = random(-speed+0.05f*speed, speed-0.05f*speed); //escoge direccion aleatoria
 
    ballIncr.y = -sqrt(sq(speed)-sq(ballIncr.x));
  }
  
 
  public void update(){
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
        ballIncr.x = -speed+0.1f*speed;
        ballIncr.y = -sqrt(sq(speed)-sq(ballIncr.x));
      } 
      else if (ballPos.x-rad < game_board.x+game_board.Width && ballPos.x-rad > game_board.x+game_board.Width/2){
        ballIncr.x = speed-0.1f*speed;
        ballIncr.y = -sqrt(sq(speed)-sq(ballIncr.x));
      }
    }//en caso del escudo, si la bola no toca la pala, rebota igualmente con eñ escudo
    else if (shield_timer != -1 && ballPos.y + rad > game_board.y){
      ballIncr.y = -abs(ballIncr.y);
    }
    else if (ballPos.y+rad > game_board.y-5 && ballPos.y+rad < game_board.y+game_board.Height/2 && ballPos.x+rad > game_board.x && ballPos.x-rad < game_board.x+game_board.Width){
      float col_pos = map(ballPos.x, game_board.x, game_board.x+game_board.Width, -speed, speed);
      if (col_pos < -speed+0.5f) col_pos = -speed+0.5f;
      if (col_pos > speed-0.5f) col_pos = speed-0.5f;
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


class Board extends Rectangle{ 
  
  public Board(float x, float y, float Width, float Height, int c){
     super(x, y, Width, Height, c);
   }
   
  public void move(){
    switch(keyCode){
      case LEFT:
      case 'A':
        if (x-board_speed >= 0){
          x-=board_speed; //mueve la raqueta siempre y cuando el movimiento no hara que la raqueta salga de la pantalla
        }
        break;
      case RIGHT:
      case 'D':
        if (x+Width+board_speed <= width){
          x+=board_speed;
        }
        break;
    }
  }
  

}


class button{ //clase botón de texto
  float x; //posion del botón
  float y;
  String text; //el texto del botón
  boolean selected; //si el raton esta encima del botón
  float textWidth; //anchura del botón
  float textHeight; //altura del botón
  int col; //color del texto
  int sel; //color del texto seleccionado
  
  public button(float x, float y, String text, int col, int sel){ //constructor con parámetros necesarios al crear un botón
    this.text = text;
    this.x = x;
    this.y = y;
    textWidth = textWidth(text);
    textHeight = textAscent();
    this.col = col;
    this.sel = sel;
  }
  
  public void update(){ //actualizar el estado del botón
    textAlign(CENTER, CENTER);
    if(mouseX >= x-textWidth/2 && mouseX <= x+textWidth/2 &&
      mouseY >= y-textHeight/2 && mouseY <= y+textHeight/2){ //si el raton está encima
      selected = true; 
      fill(sel); //se cambia el texto a color del texto seleccionado
      }
    else{ 
      selected = false;
      fill(col); //si no esta selecionado tiene el color normal
    }
    text(text, x, y); //dibuja el texto
  }
}
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
    imageMode(CENTER);
    image(sprite, x, y, sizeX, sizeY); //lo dibuja
    y+=object_speed; //incrementa su poscion
    if (y > height+5) destroyed = true; //si se ha salido de la pantalla, se destruira
    if (x+sizeX/2 >= game_board.x && x-sizeX/2 <= game_board.x+game_board.Width &&
        y+sizeY/2 >= game_board.y && y-sizeY/2 <= game_board.y+game_board.Height ){
      player_detected = true; //si el jugador lo ha cogido, hara el efecto que tiene que hacer 
    }     
  }
  
}



class Rectangle {
   float x, y; //posicion
   float Width, Height; //tamaño
   int lives, inlives; //vidas, vidas iniciales
   boolean destr; //destruido
   int c; //color
   float cpX, cpY; //punto de colision con la bola
   int colX, colY; //punto relativo de la colision

   public Rectangle(float x, float y, float Width, float Height, int c){ //constructor
     this.x = x;
     this.y = y;
     this.Width = Width;
     this.Height = Height;
     this.c = c;
     lives = 1;
     inlives = lives;
   }
   
   public Rectangle (float x, float y, float Width, float Height, int c, int lives){ //constructor que permite indicar vidas del bloque
     this.x = x;
     this.y = y;
     this.Width = Width;
     this.Height = Height;
     this.c = c;
     this.lives = lives;
     inlives = lives;
   }
   
   public void drawRect(){ //dibujar bloque
     fill(c);
     if (inlives > 1){ //si hay mas de una vida, tambien dibuja el borde
       stroke(255);
       strokeWeight(3);
     }
     else {
       noStroke();
     }
     rect(x, y, Width, Height);
   }
   
   public void hit(){ //si la pelota le da a un bloque
     lives--; //le resta vidas
     int alpha = (int)map(lives, 0, inlives, 30, 255); //la opacidad varia en funcion de vidas que quedan
     c = color(red(c), green(c), blue(c), alpha);
     if (lives <= 0 && !destr) { //si sus vidas bajan a 0, se indica que es destruido y se crea un objeto de mejora
       destr = true;
       int rand = (int)random(1, 101);
       if (rand <= 25){   //25% de probabilidad de crear un objeto
         rand = (int)random(1, 5); 
         PImage spr = null;
         switch(rand){ //crea un objeto aleatorio
           case 1:
             spr = loadImage("3ball.png");
             break;
             
           case 2:
             spr = loadImage("longer.png");
             break; 
             
           case 3:
             spr = loadImage("shorter.png");
             break;
             
           case 4:
             spr = loadImage("shield.png");
             break;
          }
          objs.add(new object(spr, rand, x+Width/2, y+Height/2, 1.5f, 1.5f)); //y lo añade a la lista
       }
     } 
   }
   
  public boolean CircleCollision(float circleX, float circleY, float circleRad){ //colision con la bola
    //(cpX, cpY) es el punto del bloque mas cercano al circulo 
    cpX = circleX; //posion del punto de colision
    cpY = circleY;
    if (circleX < x) cpX = x; //pero que no se salga del bloque
    if (circleX > x+Width) cpX = x+Width;
    if (circleY < y) cpY = y;
    if (circleY > y+Height) cpY = y+Height;
    
    float dist = sqrt(sq(cpX-circleX)+sq(cpY-circleY)); //distancia entre el circulo y el punto de colision
    
    if (dist < circleRad){ //si esa distancia es menor al radio, significa que la bola ha chocado con el bloque
      if (cpX == x) colX = 0; //guarda la posion relativa del choque (a que lado le ha dado la bola al bloque)
      else if (cpX == x+Width) colX = 1; //colX:  0 - izquierda, 1 - derecha, -1 en medio // colY: 0 - arriba, 1 - abajo, -1 en medio
      else colX = -1;
      if (cpY == y) colY = 0;
      else if (cpY == y+Height) colY = 1;
      else colY = -1;
      return true; //retorna verdad es ha chocado
    } 
    return false; //falso si no ha habido colision
  }
}
  public void settings() {  size(900, 900); }
  static public void main(String[] passedArgs) {
    String[] appletArgs = new String[] { "brickbreaker" };
    if (passedArgs != null) {
      PApplet.main(concat(appletArgs, passedArgs));
    } else {
      PApplet.main(appletArgs);
    }
  }
}
