
#define stp 2
#define dir 3
#define DISTANCE 800     //Limit: 2250 (Length of the motor-bar)
#define FULL 2250        //Full distance
#define HALF 1125

char input;
int StepCounter = 0;
int Stepping = false;
int delayH = 500;         //HIGH Step delay (Microseconds)
int delayL = 500;         //LOW Step delay  (Microseconds)


void setup() {               
  pinMode(dir, OUTPUT);
  pinMode(stp, OUTPUT);
  digitalWrite(dir, LOW);
  digitalWrite(stp, LOW);
  
  Serial.begin(9600);
}

void loop() {
  if (Serial.available()>0){
    input = Serial.read();
    if (input == '1' && Stepping == false)
    {
      Serial.println("Forward");
      forward(delayH, delayL, DISTANCE);
    }
    if (input == '2' && Stepping == false)
    {
      Serial.println("Backward");
      backward(delayH, delayL, DISTANCE);
    }
    if (input == '3' && Stepping == false) 
    {
      Serial.println("Pulse");      
      forward(1000, 1000, 800);
      delay(500);
      pulse();      
      backward(1000, 1000, 800);
      delay(500);
    }
    if(input == '4' && Stepping == false)
    {      
      Serial.println("Swipe");
      //swipe();      
      //swipe2();
      swipeUnity();
    }
    if(input == '5' && Stepping == false)
    {      
      Serial.println("Incrament");
      incrament();      
    }
    if(input == 'f' && Stepping == false){
      Serial.println("Full distance");
      forward(2225, 2225, FULL);
    }
    if(input == 'b' && Stepping == false){
      Serial.println("Back full distance");
      backward(200, 200, FULL);
    }
  }
}

void trueStep() {
  if (Stepping == true)
    {
      for (StepCounter = 0; StepCounter <= DISTANCE; StepCounter++)
      {
        digitalWrite(stp, HIGH);
        delayMicroseconds(delayH);
        digitalWrite(stp, LOW);
        delayMicroseconds(delayL);
      }
      StepCounter = 0;
      Stepping = false;      
    }
}

void trueStepVary(int high, int low, float distance) {
  if (Stepping == true)
    {
      for (StepCounter = 0; StepCounter <= distance; StepCounter++)
      {
        digitalWrite(stp, HIGH);
        delayMicroseconds(high);
        digitalWrite(stp, LOW);
        delayMicroseconds(low);
      }
      StepCounter = 0;
      Stepping = false;      
    }
}

void forward(int high, int low, float distance) {
  digitalWrite(dir, LOW);
  Stepping = true;  
  trueStepVary(high, low, distance);
}

void backward(int high, int low, float distance) {
  digitalWrite(dir, HIGH);
  Stepping = true;
  trueStepVary(high, low, distance);
}

void pulse() {
  int fwdrev = 1;
  for (int i = 0; i < 6; i++){
    if (fwdrev == 1 && Stepping == false)
    {
      digitalWrite(dir, LOW);
      Stepping = true;
      trueStep();
    }
    if (fwdrev == 0 && Stepping == false)
    {
      digitalWrite(dir, HIGH);
      Stepping = true;
      trueStep();
    }
    
    if (fwdrev == 1)
    {
      fwdrev = 0;
    }
    else
    {
      fwdrev = 1;
    }    
  }  
}

void swipe() {
  bool exit = true;
  char in;

  forward(150, 150, HALF);       
  while (exit){
    if (Serial.available()>0){
      in = Serial.read();    
      if (in == 'r' && Stepping == false){
        Serial.println("Right");
        forward(100, 100, HALF);
        delay(125);
        backward(50, 50, HALF);
      }
      if (in == 'l' && Stepping == false){
        Serial.println("Left");
        backward(100, 100, HALF);
        delay(125);
        forward(50, 50, HALF);
      }      
      if (in == 'e' && Stepping == false){
        backward(150, 150, HALF);
        exit = false;
      }
    }
  }    
}

void swipeUnity() {
  bool exit = true;
  char in;

  forward(150, 150, HALF);       
  while (exit){
    if (Serial.available()>0){
      in = Serial.read();    
      if (in == 'r' && Stepping == false){
        Serial.println("Right");
        forward(100, 100, HALF);
        
      }
      if (in == 'l' && Stepping == false){
        Serial.println("Left");
        backward(100, 100, HALF);
        
      }      
      if (in == 'e' && Stepping == false){
        backward(150, 150, HALF);
        exit = false;
      }
    }
  }    
}

void incrament() {
  bool exit = true;
  char in;
  float inc = FULL/10;
  float totalDist = 0;
       
  while (exit){
    if (Serial.available()>0){
      in = Serial.read();    
      if (in == 'u' && Stepping == false){
        Serial.println("Up");
        forward(100, 100, inc);
        totalDist += inc;
      }
      if (in == 'd' && Stepping == false){
        Serial.println("Down");
        backward(100, 100, inc);
        totalDist -= inc;
      }
      if (in == 'e' && Stepping == false){
        backward(500, 500, totalDist);
        exit = false;
      }
    }
  }    
}