import java.util.List;
import java.util.ArrayList;

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


void setup(){
  size(900, 900);
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
      color bc = colorSelector(); //elige un color aleatorio para cada bloque
      if (j%2 != 0) bricks.add(new Rectangle(brix+(briWidth+xSeparation)*i, (briy+ySeparation)*j, briWidth, briHeight, bc));
      
      else if (i <= bColumns-2)bricks.add(new Rectangle(brix+briWidth/2+(briWidth+xSeparation)*(i), (briy+ySeparation)*j, briWidth, briHeight, bc, 3));
    }
  }
  balls.add(new Ball(game_board.x+game_board.Width/2, game_board.y-15, 30));        
}


void draw(){
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

void keyPressed(){
  if (!game_pause) game_board.move(); //si se pulsa una tecla el juego no esta pausado, intentará mover la pala
  if (key == ' ' && !game_started){ //si se pulsa espacio y el juego aun no ha epezado(la bola bloqueada)
      game_started = true; //inicializa el juego
      //escoge direccion aleatoria para cada pelota en funcion del modulo de su velocidad
      for(Ball b: balls){
        b.ballIncr.x = random(-b.speed+0.1*b.speed, b.speed-0.1*b.speed);
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

void balls_movement(){
  int destr = -1;
  for(int i = 0; i < balls.size(); i++){
    balls.get(i).update();
    if(balls.get(i).destr) destr = i;
  }
  if (destr != -1) balls.remove(destr);
}


void bricks_update(){ //imprimir y borrar bloques
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

void objects_update(){ //imprimir y activar efecto de objetos
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



void window_manager(){ //imprime la pantalla correspondiente
  color col = color(255, 150, 0);
  color selCol = color (255, 255, 0);
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


void restart(){ //reinicia el juego
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
  color bc;
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

color colorSelector(){ //selector aleatorio de colores.
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
