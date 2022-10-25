mergeInto(LibraryManager.library,
{
    callTNITfunction: function () 
  {
    globals.init();
  },

  //
   ClickFunc: function (entitiId)
  {
    globals.entityClick(entitiId);
  },

  //
   BeeMeetFlower: function(beeId,flowerId)
  {
    globals.BeeMeetFlowerSend(beeId,flowerId);
  },
  
  //
   BirdMeetFruit: function(birdId, fruitId)
  {
    globals.BirdMeetFruitSend(birdId, fruitId);
  },

  //
  CreateRequestNewFruit: function(flowerId)
  {
    globals.CreateRequestNewFruitWeb(flowerId);
  },

  //
  SetFlowersLevelWeb: function(flowerId)
  {
    globals.SetFlowersLevel(flowerId);
  },

  //
  SetFruitsLevelWeb: function(fruitId)
  {
    globals.SetFruitsLevel(fruitId);
  },

  //
  TempValueSetWeb: function(tempId,value)
  {
    globals.TempValueSet(tempId,value);
  },

  //
  LightValueSetWeb: function(lightId,value)
  {
    globals.LightValueSet(lightId,value);
  },

  //
  SetBeeEnergyWeb: function(beeId,value)
  {
    globals.SetBeeEnergy(beeId,value);
  },

  

  

  

  

  

 

 


  


  

});
