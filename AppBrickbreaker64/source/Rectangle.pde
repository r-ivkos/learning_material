


class Rectangle {
   float x, y; //posicion
   float Width, Height; //tamaño
   int lives, inlives; //vidas, vidas iniciales
   boolean destr; //destruido
   color c; //color
   float cpX, cpY; //punto de colision con la bola
   int colX, colY; //punto relativo de la colision

   public Rectangle(float x, float y, float Width, float Height, color c){ //constructor
     this.x = x;
     this.y = y;
     this.Width = Width;
     this.Height = Height;
     this.c = c;
     lives = 1;
     inlives = lives;
   }
   
   public Rectangle (float x, float y, float Width, float Height, color c, int lives){ //constructor que permite indicar vidas del bloque
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
          objs.add(new object(spr, rand, x+Width/2, y+Height/2, 1.5, 1.5)); //y lo añade a la lista
       }
     } 
   }
   
  boolean CircleCollision(float circleX, float circleY, float circleRad){ //colision con la bola
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
