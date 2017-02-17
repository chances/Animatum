Animatum
========

This project is the result of my laziness and unwillingness to sit down and actually learn Blender. I personally don't want to use a full fledged 3D modeller just to animate objects for RCT3, so I created this.

Basically, a user imports an ASE model (Made in SketchUp, or whatever. The application is modeler agnostic.) and assigns meshes to bones and parents those bones to other bones, if needed. Then the user can then start animating the model by adding keyframes of different types to the animation timeline and watch their animated model take shape.

![Screenshot](https://raw.githubusercontent.com/chances/Animatum/master/dev01.png "Screenshot")

Hopefully the result is satisfactory and the workflow doesn't seem too complex. (I know it's a hell of a lot less complex than what needs to be done with Blender.)


Credits
-------

I used a few third party classes and libraries for this project. They are as follows:
+  SharpGL: On [CodePlex](http://sharpgl.codeplex.com/). MIT License, Copyright © 2011 David Kerr
+  libASE-sharp: I ported [libASE](http://interreality.sourceforge.net/software/libASE/) to C# [here](http://github.com/XESoD/libASE-sharp). LGPL License, Copyright © 2003 Peter Amstutz
+  SlimMath: On [Google Code](http://code.google.com/p/slimmath/). MIT License, Copyright © 2007-2010 SlimDX Group
+  Settings.cs: Modified from [this CodeProject](http://www.codeproject.com/Articles/15530/Quick-and-Dirty-Settings-Persistence-with-XML) article
+  VS2008MenuRenderer.cs: From [this CodeProject](http://www.codeproject.com/Articles/70204/Custom-VisualStudio-2008-style-MenuStrip-and-ToolS) article. CPOL License

The Vista Inspirat icon set is also used.
