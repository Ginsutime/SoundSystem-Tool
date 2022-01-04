Unity Audio System
-------------
**Name:** Brett Austin

  **Directory Structure:** `Unity-Audio-System/Assets/_Game/Scripts/AudioSystem/` and directories beneath it contain the entirety of the code sample. If looking for single scripts that show off large portions of the system, look at `/AudioSystem/SFXManager.cs`, `/AudioSystem/SoundPool.cs`, `/AudioSystem/MusicManager.cs`, and `/AudioSystem/MusicPlayer.cs`.

  **Explanation of Code Sample:** A self-contained, working audio system for Unity that can be plugged into existing projects and hooked up easily. Works for sound effects as well as music.
  - A) Contains a Music Manager that:
      - Instantiates itself only when a music-related command is called and is able to seamlessly continue playing music even when the scene is switched.
      - Creates an active and inactive music player in order to set up crossfading between music scriptable objects.
      - Contains methods to play and stop music on the active music player, increase and decrease layer index if there are multiple layers to a music scriptable object, and set volume. All of these merely manipulate the music player rather than handle all of the functionality, and use floats that act as fade times, meaning the time it takes for the volume of a song to gradually raise to full volume or lower to silence.
  - B) Contains a Music Player that:
      - Creates a List with a number of AudioSources representing each layer on a music scriptable object. A hard limit of 3 is implemented in MusicManager in order to limit accidental mistakes by sound designers and keep it at the maximum amount of layers needed. Can be changed in code.
      - Contains methods to play and stop music with functionality that handles assigning specific properties to the list of AudioSource layers.
      - Contains functionality to fade volume and handles it through three routines, one for stopping, one for single tracks, and one for additive tracks (meaning multiple elements playing at the same time).
  - C) Contains a SFX (Sound Effects) Manager that:
      - Instantiates itself only when a SFX-related command is called and is able to seamlessly continue playing sounds even when the scene is switched if need be.
      - Creates an organized object pooling system for looped and one shot sounds. A specificiable number of AudioSources are created and grouped under an empty parent gameobject and are used to play any looped or one shot SFX. If all AudioSources are filled and a sound needs to be played, a new pooled AudioSource is created. This cuts down on garbage collection by removing the need to delete AudioSources.
      - Contains methods to PlayOneShot and PlayLooped sounds with the majority of the functionality assigning an AudioSource's characteristics to equal fields defined in the scriptable objects able to be edited by sound designers.
      - Contains methods related to activating and disabling pooled AudioSources.
  - D) Contains a Sound Pool that handles the creation of an initial pool as well as functionality for getting, returning and creating new pool objects.
  - E) Contains Music Event script that allows sound designers to create a scriptable object from the Unity Editor project view that can be stored in a folder directory. A singular scriptable object design was chosen to consolidate all things needing to be edited by sound designers into a single location. Includes exposed fields such as:
      - Specifiable amount of music layers with an adjustable maximum of 3, able to be edited in the code of the Music Manager script.
      - Ability to choose either an additive track layer type or single track layer type.
      - Ability to specify a mixer in case types of volumes should be split or more closely controlled (Ex: Volume sliders in a main menu).
      - Master volume of the audio clip itself.
      - Specifiable float values for initial fade in time, crossfade time, and stop song fade out time.
  - F) SFX Event script that acts as an abstract base class for SFX Loop and SFX OneShot classes that implement specific methods for looped or one shot sounds. These make up the scriptable objects that sound designers can make (SFX Loop and SFX OneShot). Exposed fields of SFX Event include:
      - Specifiable amount of possible sound clips to play with one being randomly selected out of the array.
      - Ability to specify a mixer in case types of volumes should be split or more closely controlled (Ex: Volume sliders in a main menu).
      - Priority of the AudioSource, with 0 being the highest.
      - Adjustable master volume and pitch of the audio clip itself, with both featuring a ranged float slider that provides a concise way to feature minimum and maximum value ranges.
      - Adjustable stereo pan, spatial blend and attenuation minimum and maximum for 3D sounds.
      - Preview button for SFX OneShot that plays the sound in-editor, removing need to play game to test the sound with a similar implementation for SFX Loop with the addition of a Stop Preview button to stop the looped sound once the designer has heard enough repetitions of it.
      
**Date when Code was Written (Excluding Porting Finished Work into this Repo and Updates to Readme):**\
First Commit: August 26, 2021\
Final Commit: October 30, 2021
