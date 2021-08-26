# VarietyMattersDinnerTime

![Image](https://i.imgur.com/WAEzk68.png)

Update of Cozarkians mod
https://steamcommunity.com/sharedfiles/filedetails/?id=2326581524

![Image](https://i.imgur.com/7Gzt3Rg.png)

	
![Image](https://i.imgur.com/NOW7jU1.png)

A collection of optional features to improve the eating system. This is a stand alone mod, https://steamcommunity.com/sharedfiles/filedetails/?id=2207657844]Variety Matters is compatible but is **[u]NOT[/u]** required.

# Features

**[u]Meal Time[/u]** (no need to disable - just don't set the schedule)

Allows adding meals to a pawn's schedules. If it's close to dinner time, non-starving pawns will wait to eat until dinner. During meal time, pawns will eater sooner than they normally would. Pawns that don't need to eat will treat meal time as recreation time with a preference for gluttonous recreation and social relaxation. Television is highly discouraged during meal time.

**[u]Prefer Food in Dining Rooms, Hospitals, and Prisons[/u]** (disable in settings)

When selecting food, vanilla pawns prefer food that is closer to them. This mod removes the distance penalty for food that is stored in a dining room, or in the same room with a prisoner or patient. Other factors, like bonus thoughts and time to spoil may still cause pawns to select food not in a dining room. When selecting food for their inventory, pawns will still prefer the closest food.

**[u]Prefer Spoiling Food[/u]** (disable in settings)

The vanilla game gives a slight preference to food spoiling within 12 hours. This mod applies an additional sliding preference based on how close the food is to spoiling from start to finish, but only when the food is not in a freezer.

**[u]Tables Are For Meals[/u]** (disabled by default, enable in settings)

If enabled, the AteWithoutTable thought will no longer apply when pawns eat raw fruits or vegetables or certain non-meal foods like pemmican and kibble. Pawns will still look for a table, but won't get upset if they can't find one.

In addition, when taking food to their inventory, pawns will be more likely to select foods that don't require a table. This creates a distinction between pemmican (no table needed) and packaged survival meals (it's a meal, so a table is needed).

**[u]Revised AteWithoutTable Thought[/u]** (disabled by default, enable in settings)

Recommended for use with the Tables Are For Meals setting, this replaces the AteWithoutTable thought with a stacking (-3/-5/-8), longer lasting (1.5 days) thought. 

**[u]Quality Cooking[/u]** (disabled by default)

Chefs below a certain skill level have a chance of preparing cooked meals poorly. Poorly prepared meals can be too small (provide less nutrition), too large (positive mood thought + overate hediff), under/overcooked, burnt, under/overseasoned, or given a weird flavor, which result in bad thoughts of differing severities and duration.

Like food poisoning, pawns can't see if a meal was poorly prepared until they eat it. The chance for a poorly prepared meal depends on the skill of the chef. Simple meals range from a chance of 25% (level 0) to 0% (level 7+), fine meals range from 24% (level 6) to 0% (level 12+) and lavish range from 45% (level 8) to 0% (level 18+).

Poorly prepared meals stack with regular meals without contaminating the whole stack. 

**[u]Revised AteLavishMeal Thought[/u]** (disabled by default)
As a balancing option for quality cooking, to make lavish meals more viable before level 18, there is an option to change the lavish meal thought to a longer-lasting buff that fades from +12 to +8 to +4 over the course of 2 days.

**[u]Freshly Cooked[/u]**
Optional, 2x stacking mood thoughts for eating freshly cooked meals (+1) and for eating meals that have cooled to leftovers (-1) or been frozen (-2). 

You can adjust how quickly (and at what temperature) warm meals cool and how quickly they turn to leftovers. By default, warm meals turn to normal after 20 hours below 60 deg. C and turn to leftovers after 40 hours. The time is halved for refrigerated meals (below 10 deg. C). Frozen meals turn instantly to frozen leftovers.

# FAQ


**Meal Time**

Q: Doesn't https://steamcommunity.com/sharedfiles/filedetails/?id=2280250569]Real Dining already let you set dinner time?
A: Yes, but I didn't like the interaction between some of the other features of that mod and https://steamcommunity.com/sharedfiles/filedetails/?id=2207657844]Variety Matters. The food selection from Real Dining also uses a destructive prefix, so it doesn't play well with other mods that do the same thing. I originally set out to create a patch for Real Dining, but it turned out to be easier to write my own version from scratch.

**Prefer Dining Room Food**

Q: This doesn't work, pawns keep choosing food not in the dining room.
A: Use the room stat display to make sure its a dining room. If you combine your dining and rec room, the game might consider it primarily a rec room. If pawns still aren't taking food from a dining room, something must have made the other food more attractive (better mood thoughts, closer to spoiling, other mods).

Q: Doesn't https://steamcommunity.com/sharedfiles/filedetails/?id=1339148170]Room Food already make pawns prefer food in dining rooms, hospitals, and prisons?
A: Same concept, different execution. Room Food makes pawns look for food in a room before they look elsewhere, which means they will always prefer Room Food (even if it violates their restrictions). This mod increases a pawn's preference for food in a dining room, but they will still likely prefer a lavish meal in the kitchen to a fine meal in the dining room. Use whichever you prefer. Technically, they are probably compatible, but using them both would be like microwaving food that is already hot.

**Prefer Spoiling Food**

Q: Doesn't https://steamcommunity.com/sharedfiles/filedetails/?id=1561769193]Common Sense already have an option to prefer spoiling food?
A: Yes, although it uses a different formula. Use whichever you like. Alternatively, if you really hate meals spoiling, use them both.

Q: Does this work with Mod Name that also affects food choice?
A: That depends on how the other mod was coded. This will combine with other mods that tweak the vanilla system but the feature won't won't work with mods that overwrite the vanilla food selection like Smarter Food Selection and Real Dining.

**Tables Are For Meals**

Q: Does this feature make Pemmican too much better than Packaged Survival Meals?
A: That was a concern, so I tweaked the packaged survival meal recipes to make them as nutrition-efficient as pemmican (1.6), but less than simple meals (1.8). That makes sure packaged survival meals have a place as a more efficient choice for an emergency food stockpile and long caravan trips, while pemmican is great to have on hand as inventory food.

Q: Does this feature work with mod-added foods?
A: Mostly. It will find new fruits/vegetab

![Image](https://i.imgur.com/Rs6T6cr.png)



-  See if the the error persists if you just have this mod and its requirements active.
-  If not, try adding your other mods until it happens again.
-  Post your error-log using https://steamcommunity.com/workshop/filedetails/?id=818773962]HugsLib and command Ctrl+F12
-  For best support, please use the Discord-channel for error-reporting.
-  Do not report errors by making a discussion-thread, I get no notification of that.
-  If you have the solution for a problem, please post it to the GitHub repository.



