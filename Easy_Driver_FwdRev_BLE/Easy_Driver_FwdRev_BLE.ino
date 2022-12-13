
#include <ArduinoBLE.h>   //ArduinoBLE library

#define stp 2            //Setting Stepper to pin 2
#define dir 3            //Setting Direction to pin 3
#define DISTANCE 800     //Limit: 2250 (Length of the motor-bar)
#define FULL 2250        //Full distance 
#define HALF 1125        //Half distance

char input;               //Initialising input for device
int StepCounter = 0;      
int Stepping = false;
int delayH = 500;         //HIGH Step delay (Microseconds)
int delayL = 500;         //LOW Step delay  (Microseconds)

BLEService hapticService("180A"); // Bluetooth® Low Energy "Device Information" Service

// Bluetooth® Low Energy "Digital Output" Switch Characteristic - custom 128-bit UUID, read and writable by central
BLECharCharacteristic switchCharacteristic("2A57", BLERead | BLEWrite);
BLEDevice central;


void setup() {               
  pinMode(dir, OUTPUT);     //Setting direction pin as output
  pinMode(stp, OUTPUT);     //Setting stepper pin as output
  digitalWrite(dir, LOW);   //Setting both direction and stepper pins as LOW to begin
  digitalWrite(stp, LOW);
  
  Serial.begin(9600);       //Allows infromation to be seen in the serial monitor at 9600 Baud rate

  // begin initialization
  if (!BLE.begin()) {
    Serial.println("starting Bluetooth® Low Energy module failed!");

    while (1);
  }

  // set advertised local name and service UUID:
  BLE.setLocalName("Nano 33 IoT");
  BLE.setAdvertisedService(hapticService);

  // add the characteristic to the service
  hapticService.addCharacteristic(switchCharacteristic);

  // add service
  BLE.addService(hapticService);

  // set the initial value for the characeristic:
  switchCharacteristic.writeValue(0);

  // start advertising
  BLE.advertise();

  Serial.println("BLE Haptic Peripheral");
}

void loop() {
  // listen for Bluetooth® Low Energy peripherals to connect:
  central = BLE.central();

  // if a central is connected to peripheral:
  if (central) {
    Serial.print("Connected to central: ");
    // print the central's MAC address:
    Serial.println(central.address());

    Serial.println("Main Menu");
    // while the central is still connected to peripheral:
    while (central.connected()) {
            
      if (switchCharacteristic.written() ) {       
        switch (switchCharacteristic.value()) {   
          case '1':                           //Moves device forward over small distance
            Serial.println("Forward");
            forward(delayH, delayL, DISTANCE);
            break;
          case '2':                           //Moves device backward over small distance
            Serial.println("Backward");
            backward(delayH, delayL, DISTANCE);
            break;
          case '3':                           //Moves device in a pulsing motion
            Serial.println("Pulse");      
            forward(1000, 1000, 800);
            delay(500);
            pulse();      
            backward(1000, 1000, 800);
            delay(500);
            break;
          case '4':                           //Allows device to enter 'Swipe' mode
            Serial.println("Swipe");
            swipeUnityBLE();
            break;
          case '5':                           //Allows device to enter 'Inrament' mode
            Serial.println("Incrament");
            incrament(); 
            break;
          case 'f':                           //Moves device forward across full distance of threaded pole
            Serial.println("Full distance");
            forward(2225, 2225, FULL);
            break;
          case 'b':                           //Moves device backward across full distance of threaded pole
            Serial.println("Back full distance");
            backward(200, 200, FULL);
            break;
          default:
            Serial.println(F("LED off"));
            digitalWrite(LED_BUILTIN, LOW);          // will turn the LED off
            break;
        }
      }
    }

    // when the central disconnects, print it out:
    Serial.print(F("Disconnected from central: "));
    Serial.println(central.address());
  }
}
//Controlling speed and desitance of device
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
//2nd version of trueStep to allow variations in speed and distance of the device
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
//Forward travel
void forward(int high, int low, float distance) {
  digitalWrite(dir, LOW);
  Stepping = true;  
  trueStepVary(high, low, distance);
}
//Backward travel
void backward(int high, int low, float distance) {
  digitalWrite(dir, HIGH);
  Stepping = true;
  trueStepVary(high, low, distance);
}
//Pulse gesture for device
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

//Swipe gesture for device using BLE communication
void swipeBLE() {
  bool exit = true;
  char in;

  forward(150, 150, HALF);       
  while (exit){
    central.connect();
    if (switchCharacteristic.written()) {         
      switch (switchCharacteristic.value() ) {   
        case 'r':
          Serial.println("Right");
          forward(100, 100, HALF);
          delay(125);
          backward(50, 50, HALF);
          break;
        case 'l':
          Serial.println("Left");
          backward(100, 100, HALF);
          delay(125);
          forward(50, 50, HALF);
          break;
        case 'e':
          Serial.println("Exit Swipe");
          backward(150, 150, HALF);
          exit = false;
          break;
      }
    }
  }    
}
//2nd verison of Swipe gesture with BLE but adjusted for use with Haptic Demo
//made with with Unity
void swipeUnityBLE() {
  bool exit = true;
  char in;

  forward(150, 150, HALF);       
  while (exit){
    central.connect();
    if (switchCharacteristic.written()) {         
      switch (switchCharacteristic.value() ) {  
        case 'r':
          Serial.println("Right");
          forward(100, 100, HALF);          
          break;
        case 'l':
          Serial.println("Left");
          backward(100, 100, HALF);          
          break;
        case 'e':
          Serial.println("Exit Swipe");
          backward(150, 150, HALF);
          exit = false;
          break;
      }
    }
  }
  // when the central disconnects, print it out:
  Serial.print(F("Disconnected from central: "));
  Serial.println(central.address());    
}
//Incrament function which can move the device over small distance forward or backward
void incrament() {
  bool exit = true;
  char in;
  float inc = FULL/10;
  float totalDist = 0;       

  while (exit){
    central.connect();
    if (switchCharacteristic.written()) {         /*&& Stepping == false*/
      switch (switchCharacteristic.value() ) {   // any value other than 0
        case 'u':
          Serial.println("Up");
          forward(100, 100, inc);
          totalDist += inc;
          break;
        case 'd':
          Serial.println("Down");
          backward(100, 100, inc);
          totalDist -= inc;
          break;
        case 'e':
          backward(500, 500, totalDist);
          exit = false;
          break;
      }
    }
  }
}