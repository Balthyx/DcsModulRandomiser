# DcsModulRandomiser
To many dcs modules and do not want to choose ? Let it choose for you !

DcsModulRandomiser pick one module, one map and a date for the next reroll, 
If you launch it a second time it remind you your current roll.

After several days, DcsModulRandomiser will pick a new plane and a new map.

# How to use it

Launch it with the profil.json in first argument :
```
DcsModulRandomiser.exe Blueflag.json
```

You can force a choosen map :
```
DcsModulRandomiser.exe Blueflag.json forcemap Syria_Modern
```

Or force it to reroll :
```
DcsModulRandomiser.exe Blueflag.json reroll
```

# The json profil file

Customise your randomisation with the profil.json file.

The randomisation work as a tree, each branch have the same weight.
