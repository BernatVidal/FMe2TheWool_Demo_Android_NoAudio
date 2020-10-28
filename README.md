# FMe2TheWool_Demo_Android_NoAudio
 A 2D SpaceShooter demo game made in Unity


About:

Small demo and created in Unity from scratch (all resources included) within 6 days for Outfit7 ExpTest JAM on July 2020. 

All codes and resources (sprites, music, sounds, particles, UI, etc.. ) are done from scratch specially for this game, only the background image are from outside sources.

Audio resources are removed cause will be reused for commercial uses in the future.

You can check the gameplay here:

https://www.youtube.com/watch?v=bkNCO3gLshE&t=133s

/////////////////////////////////////////////////////////////

Build .apk for Android available at /Builds dir

PC build available at:

https://drive.google.com/file/d/1oFL8qEHqXKEsDw4BuDkf0iKvafubQYJV/view?usp=sharing


/////////////////////////////////////////////////////////////

Description:

Fly me to the Wool is a spaceship 2D shooter for Pc and Mobile. 
This happens on a universe where dogs and cats reached the space age. In a planet with a wool ball for moon (the Wool-Moon), live in peace and harmony a cat civilization.
They use wool balls for many purposes (not just as toys and fabric, sometimes they used also as... blasts! ), so they use to need to go to their moon to acquire some resources.
That’s where our protagonist enters the scene.
You control a cat on his travel to the Wool-Moon. The travel will take you 5 minutes, but you’ll find many hazards on your path. Space debris and dog spaceships are awaiting out there, so take care, cats only have 7 lives..!

Key Game components:

    1. Spaceship
You’ll take control of a cat and his spaceship to fly around, avoiding obstacles and enemies spaceships while shooting wool balls. 
 
    2. Enemies
The enemies are the dogs spaceships. There are 4 kinds of them: UFO dogs, Normal drones, and Big drones that also shoots Small drones with a flock swarm like behaviour. Each one with his specs and behaves.

    3. Obstacles
Years of civilization colonizing the space leaves a lot of traces behind.. In this case Space Debris that you’ll have to avoid and destroy. Each one with more or less resistance to your shots, fortunately some of them will be partially destroyed due to its own collisions. You can also try to make them collide with your shots.

    4. Powerups (weapons, shield,...)
There are 3 types of Powerups. All of them can be acquired by destroying enemies, obstacles and even enemy shoots with a small probability.

Lives : (2% drop probability). They will regenerate 1 point of your 7 lives.

Shield : (1.5% drop probability). They give you 10 seconds of protection. That’s a good thing to have when there are many enemies shooting at you. A UI indicator will appear when you’re under its effects, also a blue halo will surround you.

Triple Shot: (0.75% drop probability, after 2 minutes of playtime). This will be your best friend. Will let you shoot triple shots until you get damaged. Also a UI indicator will appear when you have it. With this your fly will be much easier, but don't let your guard down, one impact and you’ll lose it..!

    5. Scoring

There’s a scoring system implemented. Every kind of object destroyed will give you a certain amount of points, but don’t be greedy, sometimes is better preserve your live than having many points.

    6. Background environment

As it’s a spaceship shooter game, a nice space background with scrolling effects and stars moving with some parallax is required. You’ll probably don’t have many time to admire it, but it’s pretty relaxing... and infinite.

    7. Enemy behaviors (paths, formations)

Every enemy have it’s own behaviour:

Normal drones, they will come to you and shoot you. But can be avoided easily. This dogs don’t know much about IA and neural networks..

UFO Dogs will stay away from you while shooting you bone shots. They will wait until you’re on his sight. With small numbers you don’t have to worry, but don’t let them group or it will be the final thing you see.

Big drones are the bosses of this game. They will appear, shoot you and will be surrounded by swarms of small drones that will protect them. Whenever they feel they don’t have enough will spawn more, so take care!

Small drones are used to protect the big drone. They will surround him and move in a pretty interesting flock-swarm pattern. The nice part is that they’re easy to kill and they spawn in big groups, so you can easily get some powerups from them.



Game logic works around time events based on groups of spawns, it have some RNG on it so every run is different, but don’t rely on it to create the main gameplay.

The game implements some interesting background features, like a multiple pooling system 
(so the game can handle many objects and particles appearing and disappearing at the same time while avoiding unstable frame rates), or a flocking system ( implemented on the small drones that make them fly like a bee swarm), and some other code structures and ways to manage assets and audios that made a solid core for a game that could be expanded easily with some editor clicks and few code lines . 
All code is built from scratch and customized for this game without third party plugins or assets as well as all sprites and sounds (except for the mobile controls that uses the screen joystick from Unity Standard Assets package).
