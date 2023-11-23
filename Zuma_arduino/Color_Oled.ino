#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>
//rotation
#define BNO055_SAMPLERATE_DELAY_MS (100)

// Color
#include "Adafruit_TCS34725.h"

#define redpin 3
#define greenpin 5
#define bluepin 6

#define commonAnode true

byte gammatable[256];

// OLED
#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 64
// OLED
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, -1);
// Color
Adafruit_TCS34725 tcs = Adafruit_TCS34725(TCS34725_INTEGRATIONTIME_50MS, TCS34725_GAIN_4X);
//rotation
Adafruit_BNO055 bno = Adafruit_BNO055(55, 0x28, &Wire);
char displayChar_R = ' '; // 初始化为一个空格
char displayChar_L = ' '; // 初始化为一个空格
float red, green, blue;


void TCA9548A(uint8_t bus) {
  Wire.beginTransmission(0x70);  // TCA9548A address
  Wire.write(1 << bus);          // send byte to select bus
  Wire.endTransmission();
 // Serial.print(bus);
}


void displaySensorDetails(void)
{
  sensor_t sensor;
  bno.getSensor(&sensor);
}
void displaySensorStatus(void)
{
  /* Get the system status values (mostly for debugging purposes) */
  uint8_t system_status, self_test_results, system_error;
  system_status = self_test_results = system_error = 0;
  bno.getSystemStatus(&system_status, &self_test_results, &system_error);
}
void displayCalStatus(void)
{
  /* Get the four calibration values (0..3) */
  /* Any sensor data reporting 0 should be ignored, */
  /* 3 means 'fully calibrated" */
  uint8_t system, gyro, accel, mag;
  system = gyro = accel = mag = 0;
  bno.getCalibration(&system, &gyro, &accel, &mag);

  // /* The data should be ignored until the system calibration is > 0 */
  // Serial.print("\t");
  // if (!system)
  // {
  //   Serial.print("! ");
  // }
  // Serial.print("Sys:");
  // Serial.print(system, DEC);
  // Serial.print(" G:");
  // Serial.print(gyro, DEC);
  // Serial.print(" A:");
  // Serial.print(accel, DEC);
  // Serial.print(" M:");
  // Serial.print(mag, DEC);
}


void setup() {
  Serial.begin(9600);
// Start I2C communication with the Multiplexer
  Wire.begin();
  
  TCA9548A(5);//left color sensor
  Color();
  display.clearDisplay();
  
  TCA9548A(0);
  Color();
  display.clearDisplay();
  
  TCA9548A(1);
  Rotation();
  display.clearDisplay();
  
  TCA9548A(7); 
  // Initialize OLED
  if (!display.begin(SSD1306_SWITCHCAPVCC, 0x3C)) {
    Serial.println(F("SSD1306 allocation failed"));
    for (;;) {
    }
  }
  display.setTextSize(6);
  display.setTextColor(WHITE);
  display.clearDisplay();
}

void Color() {
  if (tcs.begin()) {
  } else {
    Serial.println("No TCS34725 found ... check your connections");
  }

#if defined(ARDUINO_ARCH_ESP32)
  ledcAttachPin(redpin, 1);
  ledcSetup(1, 12000, 8);
  ledcAttachPin(greenpin, 2);
  ledcSetup(2, 12000, 8);
  ledcAttachPin(bluepin, 3);
  ledcSetup(3, 12000, 8);
#else
  pinMode(redpin, OUTPUT);
  pinMode(greenpin, OUTPUT);
  pinMode(bluepin, OUTPUT);
#endif

  for (int i = 0; i < 256; i++) {
    float x = i;
    x /= 255;
    x = pow(x, 2.5);
    x *= 255;

    if (commonAnode) {
      gammatable[i] = 255 - x;
    } else {
      gammatable[i] = x;
    }
  }
}

void Rotation(){
  while (!Serial) delay(10);  // wait for serial port to open!
  Serial.println("Orientation Sensor Test"); 
  Serial.println("");
  if(!bno.begin())
  {
   Serial.print("Ooops, no BNO055 detected ... Check your wiring or I2C ADDR!");
    while(1);
  }
  delay(10);
  displaySensorDetails();
  displaySensorStatus();
  bno.setExtCrystalUse(true);
}

  
void loop() {
  TCA9548A(5);
  Getcolor_left();
  changecolor_L();
  
  TCA9548A(0);
  Getcolor_R();
  changecolor_R();
  
  TCA9548A(1);
  Getrotation();
  
  TCA9548A(7);
  OLED_1();
}



void Getcolor_R(){
  tcs.setInterrupt(false);  // 打开LED
  delay(60);  // 读取颜色需要50ms
  tcs.getRGB(&red, &green, &blue);
  tcs.setInterrupt(true);  // 关闭LED

//  Serial.print("R:\t");
//  Serial.print(int(red));
//  Serial.print("\tG:\t");
//  Serial.print(int(green));
//  Serial.print("\tB:\t");
//  Serial.print(int(blue));
//  Serial.print("\n");
//  delay(1000); // 为了显示稳定，可以根据需要调整延迟时间。
}

void Getcolor_left(){
  tcs.setInterrupt(false);  // 打开LED
//  delay(60);  // 读取颜色需要50ms
  tcs.setInterrupt(true);
  tcs.getRGB(&red, &green, &blue);
//  tcs.setInterrupt(true);  // 关闭LED

  // Serial.print("R:\t");
  // Serial.print(int(red));
  // Serial.print("\tG:\t");
  // Serial.print(int(green));
  // Serial.print("\tB:\t");
  // Serial.print(int(blue));
  // Serial.print("\n");
 // delay(1000); // 为了显示稳定，可以根据需要调整延迟时间。
}



void changecolor_R(){
  if (red > green && red > blue) {
    displayChar_R = 'R'; 
  } else if (green > red && green > blue) {
    displayChar_R = 'G'; 
  } else if (blue > red && blue > green) {
    displayChar_R = 'B'; 
  }
//  Serial.print(displayChar_R);
//  Serial.print("\n");
}

void changecolor_L(){
  if (red > green && red > blue) {
    displayChar_L = 'R'; 
  } else if (green > red && green > blue) {
    displayChar_L = 'G'; 
  } else if (blue > red && blue > green) {
    displayChar_L = 'B'; 
  }
//  Serial.print(displayChar_R);
//  Serial.print("\n");
}

void OLED_1() {
  display.clearDisplay();
  display.setCursor(10, 10);
  display.print(displayChar_L);
  display.print(',');
  display.print(displayChar_R);
  display.display();
}


void Getrotation(){
  sensors_event_t event;
  bno.getEvent(&event);

  //Serial.print("\tY: ");
  Serial.println(event.orientation.y, 4);

  /* Optional: Display calibration status */
  displayCalStatus();

//   Serial.println("");
//  /* Wait the specified delay before requesting nex data */
 // delay(BNO055_SAMPLERATE_DELAY_MS);
  }
