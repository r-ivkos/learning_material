

class Board extends Rectangle{ 
  
  public Board(float x, float y, float Width, float Height, color c){
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
