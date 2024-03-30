local success = context.GetVariable("final-0") and
  context.GetVariable("final-1") and
  context.GetVariable("final-2") and
  context.GetVariable("final-3")

local any = context.GetVariable("final-0") or
  context.GetVariable("final-1") or
  context.GetVariable("final-2") or
  context.GetVariable("final-3")

if any and not success then
  context.WriteInfo("The sound of meat reforming can be heard in the showroom.")
  context.SetVariable("respawn-meatballs", true)
  context.SetVariable("final-count", 0)
  context.SetVariable("final-0", false)
  context.SetVariable("final-1", false)
  context.SetVariable("final-2", false)
  context.SetVariable("final-3", false)
end