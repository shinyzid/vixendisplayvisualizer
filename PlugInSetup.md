# Details #

Here you will choose the channels you want to use fir the plugin. this way you could do your entire show, or just a particular element, etc. This is also where you would define your RGB pixels. For each RGB pixel, you would have to assign a channel to the red, the green, and the blue portion of the pixel. I should probably support RGB+W, while I am at it (for the Mighty Mini's, etc).

This is also where it will allow you to set a picture as the background. This will be the canvas where different componets can be placed. There will be a place to add or remove components (as well as importing them, more on that later).

Each component will be a grid. The size of the grid will be user configurable, x wide, by y tall. This will allow the user to be as coarse or granular as needed for the component. Each cell in the grid can be assigned to 1 channel, or the above mentioned RGB pixel. In the case of a single channel, it will use the color assigned to it in Vixen. If it is a RGB pixel, it will display a RGB, or RGB+W gradient in the setup module (when actually running, I want it to display the actual color the RGB should make). A cell can be copied or cut, and then pasted into a new cell, or range or cells.

There will be an Import/Export Template. This will allow you to export the current state of the component. So lets say someone makes a template for a sawyer star, they could export it, and another user coul import it, map their vixen channels to the channels used in the plug-in, place it in the display canvas and it would be ready to go!