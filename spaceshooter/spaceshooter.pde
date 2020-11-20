import java.util.List;
import java.util.ArrayList;

boolean game_pause; //indica si el juego esta o no en marcha
int game_window; //la ventana que esta abierta en ese momento 
int player_pos = 3; //el primer componente de la matriz de sprites de la nave del jugador, indica la animacion al girar
int player_state = 3; //el segundo componente que indica la animacion de las turbinas (el fuego de atras de la nave)
int pSpr; //indice del sprite de la nave del jugador
int pLives; //vidas del jugador
int shSpeed; //velocidad de disparo
int prLvl; //cantidad de proyectiles al disparar
int points;//puntos que consigue el jugador
int pPoints; //el ultimo valor de puntos despues de subir la dificultada (para subir la dificultad cada X puntos)
int difficulty; //dificultad del juego
boolean superPower; //estado de invencibilidad temporal que le da un objeto a la nave
PImage player[][]= new PImage[player_pos][player_state]; //la matriz con sprites de la nave del jugador
PImage ship; //sprite que tiene actualmente la nave
PImage background; //imagen del fondo de la pantalla
float pX;  //posicion X del jugador
float pY; //posicion Y del juagodr
float bgX; //posicion X del fondo (el fondo va moviendose)
float bgY; //posicion Y del fondo
float proj_speed = 20; //velocidad de los proyectiles que dispara la nave
float enemy_speed = 2; //velocidad de caida de los enemigos
//float e1lives; //vidas que tiene el primer enemigo
//float e2lives; //vidas que tiene el segundo tipo de enemigo
float object_speed = 4; //velocidad de caida de los objectos recogibles
int enemy_timer, pTimer, proj_timer, spower_timer; //temporizadores necesarios para repetir el codigo cada X tiempo
List<enemy> enemies = new ArrayList<enemy>(); //"Array modificable" de enemigos,
List<projectile> proj = new ArrayList<projectile>(); //de proyectiles disparados
List<object> obj = new ArrayList<object>(); // y de objetos caidos
PFont font[] = new PFont[2]; //una array de fuentes para Arial y Arial negrita
void setup(){
  size(900, 900); //tamaño de la ventana del juego
  background = loadImage("bg.png"); // cargar la imagen del fondo
  game_pause = true; //inicialmente el juego no esta en marcha ya que aparece la ventana inicial
  game_window = 1; //inicialmente se muestra el menu
  proj_timer = 0; //incialmente temporizadores estan en 0
  enemy_timer = 0;
  pLives = 3;
  shSpeed = 1;
  prLvl = 1;
  pTimer = 0;
  points = 0;
  pPoints = 0;
  difficulty = 1;
  font[0] = createFont("Arial", 1); //indico las fuentes
  font[1] = createFont("Arial Bold", 1);
  for(int i = 0; i < player_pos; i++){
    for(int y = 0; y < player_state; y++){
      player[i][y] = loadImage("\\player\\ss"+i+"b"+y+".png"); //cargar los sprites del jugador en la matriz
    }
  } 
  pSpr = 0;
  ship = player[pSpr][1]; //indicar el sprite inicial y posiciones iniciales
  bgX = width/2;
  bgY = height/2;
  pX = width/2;
  pY = height-player[0][1].height*2;
}

void draw(){
  imageMode(CENTER); //Los puntos (x,y) de las imagenes son el centro de la imagen
  if (!game_pause){
  bgMovement(); //se encarga del moviemento del fondo
  projMov(); //se encarga de disparar, mover y eliminar proyectiles los proyectiles
  enemyMov();//genera, mueve y mata enemigos (y crea objeto si un enemigo se ha muerto)
  objMov(); //mueve y borra objetos
  playerMovement(); //control y moviemento de la nave del jugador
  showStats(); //muestra informacion (como vidas, puntos, etc.);
  }
  else if (game_window <= 2){
    showMenu(); //mostrar el menu inicial
  }
  else if (game_window == 4){
    gameOver();
  }
  
}

void bgMovement(){ //se encarga del moviemento del fondo
  //se dibujan 2 fondos que van bajando
  image(background, bgX, bgY, width, height);
  image(background, bgX, bgY - height, width, height);
  bgY+=1;
  if(bgY >= height + height/2){ // cuando la imagen de inicial deja de mostrarse en la ventana, vuelve a su posicion inicial
    bgY = height/2;
  }  
}
void playerMovement(){
  float incr = mouseX - pmouseX; 
  if (incr < 0) pSpr = 1; //mira si el desplazamiento del raton es a la derecha/izquierda o no hay desplazamiento
  else if (incr > 0) pSpr = 2; //y carga la imagen de giro correspondiente
  else pSpr = 0; //
  pX = mouseX; //posicion X del jugador es la del raton (no se mueve verticalmente)
  if (timer(0.15, pTimer)){ //cada 0.15 segundos cambia sprite para la animacion del fuego de la nave
    pTimer = millis();
      if (ship == player[pSpr][1]) ship = player[pSpr][2];
      else ship = player[pSpr][1];
    }
  image(ship, pX, pY, ship.width*2, ship.height*2); //finalmente dibuja la nave
}


void projMov(){
  float a_speed = 0.025*10; //el timpo de disparo que restarle al inicial
  if (shSpeed <= 10) a_speed = 0.025*(shSpeed-1);
  if (mousePressed && (timer(0.3-a_speed, proj_timer) || superPower)){ //si el raton esta pulsado cada x segundos (depende del nivel de la velocidad de disparo) se crea un proyectil
    proj_timer = millis(); 
    PImage proj_img = loadImage("\\projectiles\\laser.png"); //carga el sprite del proyectil
   if(!superPower){ 
       switch(prLvl){ //Se dibuja la cantidad de proyectiles igual al nivel de "cantidad de proyectiles"
        case 1:
           //se añade a la lista un proyectil
           proj.add(new projectile(proj_img, pX, pY-(ship.height/2*2)/2, proj_speed, 2, 2));
           break;
         
        case 2:
          proj.add(new projectile(proj_img, pX-(ship.width/2*2)/3, pY-(ship.height/2*2)/2, proj_speed, 2, 2));
          proj.add(new projectile(proj_img, pX+(ship.width/2*2)/3, pY-(ship.height/2*2)/2, proj_speed, 2, 2));
          break;
        
       case 3:
          proj.add(new projectile(proj_img, pX, pY-(ship.height/2*2)/2, proj_speed, 2, 2));
          proj.add(new projectile(proj_img, pX-(ship.width/2*2)/3, pY-(ship.height/2*2)/2.5, proj_speed, 2, 2));
          proj.add(new projectile(proj_img, pX+(ship.width/2*2/3), pY-(ship.height/2*2)/2.5, proj_speed, 2, 2));
          break;
       case 4:
          proj.add(new projectile(proj_img, pX-(ship.width/2*2)/4, pY-(ship.height/2*2)/2, proj_speed, 2, 2));
          proj.add(new projectile(proj_img, pX+(ship.width/2*2)/4, pY-(ship.height/2*2)/2, proj_speed, 2, 2));
          proj.add(new projectile(proj_img, pX-(ship.width/2*2)/1.5, pY-(ship.height/2*2)/2, proj_speed, 2, 2));
          proj.add(new projectile(proj_img, pX+(ship.width/2*2)/1.5, pY-(ship.height/2*2)/2, proj_speed, 2, 2));
          break;
       default:
         if (prLvl >= 5){ //si prLvl es 5 o mas, se alcanza el nivel maximo, por lo que a partir de 5 siempre se dispararan 5 proyectiles
          proj.add(new projectile(proj_img, pX, pY-(ship.height/2*2)/2, proj_speed, 2, 2));
          proj.add(new projectile(proj_img, pX-(ship.width/2*2)/3, pY-(ship.height/2*2)/2.5, proj_speed, 2, 2));
          proj.add(new projectile(proj_img, pX+(ship.width/2*2)/3, pY-(ship.height/2*2)/2.5, proj_speed, 2, 2));
          proj.add(new projectile(proj_img, pX-(ship.width/2*2)/1.5, pY-(ship.height/2*2)/3, proj_speed, 2, 2));
          proj.add(new projectile(proj_img, pX+(ship.width/2*2)/1.5, pY-(ship.height/2*2)/3, proj_speed, 2, 2));
         }
         break;
      }
   }  
   else {
    proj_img = loadImage("\\projectiles\\slaser.png");
    proj.add(new projectile(proj_img, pX, pY-(ship.height/2*2)/2, proj_speed, 2, 2));
    proj.add(new projectile(proj_img, pX-(ship.width/2*2)/3, pY-(ship.height/2*2)/2.5, proj_speed, 2, 2));
    proj.add(new projectile(proj_img, pX+(ship.width/2*2)/3, pY-(ship.height/2*2)/2.5, proj_speed, 2, 2));
    proj.add(new projectile(proj_img, pX-(ship.width/2*2)/1.5, pY-(ship.height/2*2)/3, proj_speed, 2, 2));
    proj.add(new projectile(proj_img, pX+(ship.width/2*2)/1.5, pY-(ship.height/2*2)/3, proj_speed, 2, 2));
   }
}
  int destr_index = -1; //posicion en la lista - si es el caso - del proyectil destruido (-1 -> no hay proyectiles que destruir)
  for (int i = 0; i < proj.size(); i++){ //para cada proyectil en el juego se actualiza su estado
    projectile projec = proj.get(i); //creo un objecto que es  identico al de la lista para poder trabajar con el
    if (projec.destroyed) destr_index = i; //si esta destruido, se guarda la posicion que tiene en la lista 
    else projec.update(); //si no, se actualiza (incrementa su posicion y comprueba si se puede considerar destruido o no (colision con enemigos/salir del mapa))
  }
  if (destr_index != -1)proj.remove(destr_index); //si hay un proyectil destruido, se borra de la lista y, por tanto, del juego
}
void enemyMov(){
  int died_index = -1; //posicion en la lista - si es el caso - del enemigo muerto (-1 -> no hay enemigos muertos)
  
  if(timer(1, enemy_timer)){ //cada segundo aparece un nuevo enemigo
    enemy_timer = millis();
    PImage enemy_img = loadImage("\\enemies\\flowey.png"); 
    enemies.add(new enemy(enemy_img, 1, difficulty, 1.75, 1.75)); //se añade a la lista un nuevo enemigo
    int rand = (int)random(1,101); //un numero aleatrio entre 1 y 101 (1 incluido, 101 no incluid)
    if (rand < 20){ //asi se hace una probabilidad de 20%, por tanto ademas del enemigo facil, hay un  20% de probabilidad de aparecer un enemigo mas fuerte 
      PImage strong_enemy_img = loadImage("\\enemies\\sans.png");
    enemies.add(new enemy(strong_enemy_img, 2, 3+difficulty, 1.75, 1.75)); //se crea el enemigo fuerte
    }
  }
  for(int i = 0; i < enemies.size(); i++){ //actualizar el estado de cada enemigo en el juego
    enemy enem = enemies.get(i); //como con los proyectiles, se crea el objeto para trabajar con el
    if (!enem.dead)enem.update(); //si el enemigo no esta muerto se actualiza su posicion y se dibuja
    else if (enem.dead){ //y si esta muerto:
      int rand = (int)random(1, 101); //se hace una probabilidad para ver si el enemigo deja o no un objeto
      if ((rand <= 5+3*difficulty && enem.type == 1) || (rand <=30+2*difficulty && enem.type == 2)){ //5% si el enemigo era facil, 30% si el enemigo era dificl y va subiendo con la dificultad
        PImage obj_spr = null; //el sprite del objeto
        rand = (int)random(1, 10); 
       if (!superPower){ //se crea un objeto aleatorio mientras la nave no este en estado de super poder
           switch (rand){
            case 1: case 2: 
              obj_spr = loadImage("\\objects\\projup.png"); 
              obj.add(new object(obj_spr, 1, enem.x, enem.y));
              break;
            case 3: case 4:
              obj_spr = loadImage("\\objects\\speedup.png");
              obj.add(new object(obj_spr, 2, enem.x, enem.y));
              break;
            case 5: case 6:
              obj_spr = loadImage("\\objects\\live.png");
              obj.add(new object(obj_spr, 3, enem.x, enem.y));
              break;
            case 7: case 8:
              obj_spr = loadImage("\\objects\\bheart.png");
              obj.add(new object(obj_spr, 4, enem.x, enem.y));
              break;
            case 9: 
              obj_spr = loadImage("\\objects\\superpower.png");
              obj.add(new object(obj_spr, 5, enem.x, enem.y));
              break;
          }
        }
      }
     died_index = i; //y finalmente, se guarda la posicion en la lista del enemigo
    }
    
  }
  if (died_index != -1)enemies.remove(died_index); //si hay un enemigo muerto, se borra de la lista y, por tanto, del juego
}
void objMov(){ //mueve y borra objetos
  int destr_index = -1; //posicion en la lista - si es el caso - del objeto destruido (-1 -> no hay objetos destruidos)
  for(int i = 0; i < obj.size(); i++){//actualizar el estado de cada objeto en la lista
    object objt = obj.get(i); //se crea un objeto para trabajar con los de la lista
    objt.update(); //se actualiza su posicion
    if (objt.destroyed) destr_index = i; //si el objeto esta destruido se guarda la posicion en la que esta en la lista
    if (objt.player_detected){ //si el jugador lo recoge:
      destr_index = i; // se guarda la poscion en la lista para borrarlo despues
      switch (objt.type){ //y dependiendo del tipo de objeto se aplican mejoras 
        case 1: 
          if (prLvl < 5) prLvl++; 
          break;
        case 2:
           if(shSpeed <10) shSpeed++;
          break;
        case 3:
          if (pLives < 3) pLives++;
          break;
        case 4:
          pLives--;
          shSpeed-=2;
          if (prLvl-2 >= 1)prLvl-=2;
          break;
        case 5:
          superPower = true;
          spower_timer = millis();
          break;
      }
    }
  }
  if (destr_index != -1)obj.remove(destr_index); //finalmente se borra de la lista y, por tanto, del juego un objeto destruido 
  if (superPower && timer(10, spower_timer)){ //si el jugador ha pillado el super poder y han pasado 10 segundos, se anula el efecto.
    superPower = false;
  }
}

void showStats(){//muestra informacion
  imageMode(CORNER); //Los puntos (x,y) de las imagenes son la esquina de arriba izquierda
  if (pLives < 1){ //se acaba el juego cunado el jugador pierde todas las vidas
    game_pause = true;
    game_window = 4;
  }
  else image(loadImage("\\player\\lives"+pLives+".png"), 10, 10); //dibuja los corazones que indican cuanta vida le queda la jugador
  //textMode(CENTER);
  textSize(60); //cambia la fuente del texto que se muestra
  textFont(font[0], 30);
  fill(255);
  text("Points: "+points, width -150, 40);  //muestra la cantidad de puntos que ha conseguido el jugador
  imageMode(CENTER); //restablece el valor de imageMode
  if (points - pPoints >= 33){ //la dificultad sube cada 33 puntos
    difficulty++;
    pPoints = points;
  }
}

void showMenu(){
  color col = color(200, 200, 100);   //color para el texto
  color selCol = color(255, 255, 200); //color para el texto seleccionado (botones)
  image(background, width/2, height/2); 
  ship = player[0][0];
  if (game_window == 1){ //el menu inicial
    image(ship, pX, pY, 2*ship.width, 2*ship.height);
    textFont(font[1], 60); //establecer la fuente y el tamaño del texto
    textAlign(CENTER, CENTER); //puntos X Y del texto son el centro del texto
    fill(255, 255, 100); //color del texto
    text("SPACESHOOTER", width/2, 100);
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
  }
  else if (game_window == 2){ //segunda ventana del menu 
    textFont(font[0], 30);
    fill(255, 255, 100);
    textAlign(LEFT, CENTER);
    textLeading(30); //separacion entre lineas de texto
    image(ship, 70, 100, 2*ship.width, 2*ship.height);
    text("Esta es tu nave. La nave sigue el ratón\npero solo se mueve horizontalmente.\nPara disparar mantén click izquierdo en el ratón.", 120, 90);
    PImage en1 = loadImage("\\enemies\\flowey.png"); 
    PImage en2 = loadImage("\\enemies\\sans.png");
    image(en1, 70, 200, en1.width*1.75, en1.height*1.75);
    image(en2, 70, 250, en2.width*1.75, en2.height*1.75);
    text("Estos son los enemigos, tu objetivo es matarlos.\nSi llegan abajo pierdes una vida.", 120, 220);
    image(loadImage("\\objects\\live.png"), 70, 350);
    text("Te suma una vida. Pero solo puedes tener un máximo \nde 3 vidas.", 120, 340);
    image(loadImage("\\objects\\speedup.png"), 70, 450);
    text("Tu nave disparará más rapidamente.", 120, 440);
    image(loadImage("\\objects\\projup.png"), 70, 550);
    text("Tu nave dispará más balas a la vez.", 120, 540);
    image(loadImage("\\objects\\bheart.png"), 70, 650);
    text("Te baja una vida, velocidad de disparo\ny balas por disparo.", 120, 640);
    image(loadImage("\\objects\\superpower.png"), 70, 740);
    text("???", 120, 740);
    textFont(font[1], 40);
    button b1 = new button(800, 800, "VOLVER", col, selCol);
    b1.update();
    if (b1.selected && mousePressed){ //si se pulsa el boton se vuelve al la ventana principal
      game_window = 1;
    }
  }
}

void gameOver(){ //pantalla de game over
  color col = color(200, 200, 100);   //color para el texto
  color selCol = color(255, 255, 200); //color para el texto seleccionado (botones)
  image(background, width/2, height/2);
  textFont(font[1], 60);
  fill(255, 150, 150);
  textAlign(CENTER, CENTER);
  text("GAME OVER", width/2, 200);
  fill(255, 255, 100);
  textFont(font[0], 40);
  if (points == 1) text("Solo has conseguido un punto", width/2, 300);
  else if (points > 1) text("Has conseguido " + points + " puntos", width/2, 300);
  else text("No has conseguido puntos...", width/2, 300);
  textFont(font[1], 40);
  button b1 = new button(width/2, 500, "VOLVER A JUGAR", col, selCol);
  button b2 = new button(width/2, 600, "SALIR", col, selCol);
  b1.update();
  b2.update();
  if (b1.selected && mousePressed){
    restart();
  }
  if (b2.selected && mousePressed){
    exit();
  }
}

void restart(){ //reiniciar el juego
  obj.clear(); //se borran todos los objetos del la lista / del juego
  proj.clear();
  enemies.clear();
  proj_timer = millis(); //se guarda el momento en el que empieza el juego
  enemy_timer = millis();
  pTimer = millis();
  //las demas variables se igualan a su valor inicial (el mismo que se le asigna en el setup)
  pLives = 3;
  shSpeed = 1;
  prLvl = 1;
  points = 0;
  pPoints = 0;
  difficulty = 1; 
  pSpr = 0;
  ship = player[pSpr][1]; 
  bgX = width/2;
  bgY = height/2;
  pX = width/2;
  game_window = 3;
  game_pause = false;
   
}

void keyPressed(){
  if (!game_pause && key == TAB && game_window == 3){ //pausa del juego
    game_pause = true;
    noLoop();
    fill(200, 200, 0);
    textFont(font[1], 60);
    textAlign(CENTER, CENTER);
    text("PAUSE", width/2, height/2);
  }
  else if (game_pause && key == TAB && game_window == 3){ //si el juego ya esta pausado, quita la pausa
    game_pause = false;
    loop();
  }
}
boolean timer(float seconds, int timer){ //funcion temporizador
  return(millis()-timer >= seconds*1000); //retorna true si ha pasado el tiempo que hay que esperar
}
