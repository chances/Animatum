Animatum
========

This project is the result of my laziness and unwillingness to sit down and actually learn Blender. I personally don't want to use a full fledged 3D modeller just to animate objects for RCT3, so I created this.

Basically, a user would import an ASE model (Made in SketchUp, or whatever. The application is modeller agnostic.) and begin assigning meshes to bones and parent those bones to other bones if needed. Then the user could start animating the model by adding keyframes of different types to the animation timeline and watch their animated model take shape.

A screen shot could best illustrate this:
![Screenshot](http://i204.photobucket.com/albums/bb63/xavier0794/Animatum/dev01.png "Screenshot")

Hopefully the result is satisfactory and the workflow doesn't seem too complex. (I know it's a hell of a lot less complex than what needs to be done with Blender.)

Anyway, this repository now houses this project. Enjoy.


Credits
-------

I used a few third party classes and libraries for this project. They are as follows:
+  SharpGL: On [CodePlex](http://sharpgl.codeplex.com/)
+  libASE-sharp: I ported [libASE](http://interreality.sourceforge.net/software/libASE/) to C# [here](http://github.com/XESoD/libASE-sharp)
+  VS2008MenuRenderer.cs: From this [CodeProject](http://www.codeproject.com/Articles/70204/Custom-VisualStudio-2008-style-MenuStrip-and-ToolS) article