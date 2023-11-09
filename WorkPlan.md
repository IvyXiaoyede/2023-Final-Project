# Summer
### Research
After observation, I found that in normal games, colour is a very important way to assist the expression of game concepts, but after my observation and research, I found that there is no special assistive device that can help colour blind people to achieve a better gaming experience, although there is a similar product like glasses for colour blind people exists, but it doesn't provide much help for the game, so I wanted to design So I want to design a wearable product that can assist colour blind people to play games and get in a better gaming mood.
In my design, I need two parts, a personal computer and a wearable device part, the personal computer mainly displays the game scene, just like a normal screen to display normal games, the device part is mainly a controller, mainly to achieve the purpose of controlling the game, and to assist colour-blind people to distinguish colours is also done through the controller.
#### PC：
During the summer holidays I had intended to make a game using PAC-MAN, when I copied the game and processed it I found that PAC-MAN did not run as well as I had initially expected (pictures) and the game did not rely on colours as much as it should. Therefore, after discussing this with my teacher, I decided to change the game.

#### Wearable part：
When I started using wearables and trying out handheld wearables, I found it a bit difficult to control the mouse and the colour sensor with one hand at the same time. Therefore, I intend to integrate both functions directly into one wearable, using my right hand to control the rotation, launching and drawing the colours of the launching ball, while my left hand will be responsible for drawing the colours of the moving ball. This way, I can have all the necessary controls on both left and right handed wearables.

# Week 1
### 1. Finding the right new game-zuma 
I simply processed the colour tones in Ps and found that zuma's gameplay is very colour dependent. I simply processed the game screenshots (fig 1,2,3) and you can see that after removing all the RGB colours, zuma is very much affected by colour, which basicallymakes it a very poor gaming experience for colourblind people.
#### Regular（Figure 1）
![Zuma_Regular](https://github.com/IvyXiaoyede/2023-Final-Project/assets/119190967/6a9a2396-5cce-41af-8c45-3f144a757996)
#### Red Miss（Figure 2）
<img width="829" alt="Zuma_Red" src="https://github.com/IvyXiaoyede/2023-Final-Project/assets/119190967/ce67159c-01a2-4776-85d8-0f678b53855d">

#### Color Blindness（Figure 3）
<img width="835" alt="Zuma_grey" src="https://github.com/IvyXiaoyede/2023-Final-Project/assets/119190967/f227e9b2-2998-4612-8ec2-751d7f1a2c8e">

  
### 2.Determining the base unit sensors and testing connections

This week I have been working on the electronic connection of the sensing device for the base of the game, for the launch angle control issue
I worked on it using the MPU6050 to solve the problem of detecting the angle of rotation and connecting the button for the purpose of controlling the launcher.

<img width="644" alt="image" src="https://github.com/IvyXiaoyede/2023-Final-Project/assets/119190967/622ec32f-39fa-4ecc-bd29-08531e2aa3dc">

# Week 2
This week, I worked on the open source project to clone the Zuma game. Firstly, I completed the overall copy and then worked on improving the details. In the process, I checked out a few websites for more inspiration and optimisation ideas.

https://leetcode.com/problems/zuma-game/

https://www.youtube.com/watch?v=3gSiFNR4C-8

https://github.com/GalaxyShad/Zuma-Deluxe-HD

https://github.com/CosmicCrash/ZumaBlitzRemake

# Week 3
This week, my main task was to connect and test colour sensors for wearable devices. Firstly, I scrutinised the information about colour sensors on the website and then carried out a series of simple but effective tests. I try to use the TCS3200 RGB, but its not good for my project.
<img width="380" alt="image" src="https://github.com/IvyXiaoyede/2023-Final-Project/assets/119190967/e8b07421-f4cf-4a58-b8f4-b10a046a0509">
<img width="419" alt="image" src="https://github.com/IvyXiaoyede/2023-Final-Project/assets/119190967/10a0643b-29c5-4bed-a1db-bf98afbbfd10">

<img width="488" alt="image" src="https://github.com/IvyXiaoyede/2023-Final-Project/assets/119190967/4d83db0b-8efb-411c-8bb8-c6aa9222312d">

In addition to this, I also took the time to focus on the required research section of the report. Through extensive research, I dug deeper into the information and fleshed out this section to ensure a richer and more detailed report.
# Week 4
1.我已经解决rotation的控制问题，并且选择z轴作为我的控制变量，最终我选择的Rotation sensor is BNO 055。

2.经过测试原来的color sensor 显示效果并不理想，所以我使用了其他的color sensor，识别效果好了许多，但对于识别屏幕还是会有一些色差。

3.提出新的想法，想在screen上显示color sensor 提取到的颜色，分别以R G B三个字母对应 Red Green Blue 三个颜色。
# Week 5
1.The teacher's suggestion sounds interesting! The right hand controls the colour of the spinning and launching ball, while the left hand is responsible for sucking up the colour of the moving ball, so you need both hands to work in tandem to identify the two colours.

2.After testing, the colours of the balls in unity are not pure enough, so I should change the colour values of the balls to make them more recognizable. I changed the colour of the blue ball and the green ball to be easily recognizable by the colour sensor to get the data.

3.Since my device is spread across both left and right handers, Hadeel suggested that I try to use two Arduino boards to control it and try to use Bluetooth for connectivity to transfer the data, and during the week I first did that by figuring out how to use one Arduino board to get the data from the two colour sensors and display it together.

4.Because I want two colour sensors back to be displayed on two screens, at hadeel's suggestion, I'm displaying both colours on one screen. I have implemented this function
You can see that using two colour sensors to read the colour data separately in the screen is implemented to read the data from two colour sensors in one screen.
<img width="842" alt="image" src="https://github.com/IvyXiaoyede/2023-Final-Project/assets/119190967/a7313e6e-9b7d-4dfb-bcb2-8e3fc961652f">


# Week 6
1. 在技术指导中，老师给我推荐了这个传感器，可以使我原本的线段变得更加简洁明了。
   https://how2electronics.com/getting-started-with-seeed-xiao-ble-nrf52840-sense/
   使用