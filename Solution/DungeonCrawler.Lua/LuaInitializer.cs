//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.9174
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// ----------------------------------------------------------
// Generated by MoonSharp.Hardwire v.1.0.6131.34058
// Compatible with MoonSharp v.2.0.0.0 or equivalent
// ----------------------------------------------------------
// Code generated on 2024-03-13T11:40:31.5152568-07:00
// ----------------------------------------------------------
// 
// 
// 
namespace CaptainCoder.Dungeoneering.Scripting {
    
    
    public abstract class LuaInitializer {
        
        private LuaInitializer() {
        }
        
        public static void Initialize() {
            MoonSharp.Interpreter.UserData.RegisterType(new TYPE_c46c3d21c1e44feea05351db8495f468());
            MoonSharp.Interpreter.UserData.RegisterType(new TYPE_373c1f0427c24695ac0513bb4ed27118());
        }
        
        #region Descriptor of DungeonCrawler.Lua.LuaAPI
        // Descriptor of DungeonCrawler.Lua.LuaAPI
        private sealed class TYPE_c46c3d21c1e44feea05351db8495f468 : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredUserDataDescriptor {
            
            internal TYPE_c46c3d21c1e44feea05351db8495f468() : 
                    base(typeof(DungeonCrawler.Lua.LuaAPI)) {
                this.AddMember("__new", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("__new", typeof(DungeonCrawler.Lua.LuaAPI), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_d7a7856314364c83811c7745aff813a3()}));
                this.AddMember("Sum", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("Sum", typeof(DungeonCrawler.Lua.LuaAPI), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_1c6491be3ab14863ac6310046e2dc881()}));
                this.AddMember("GetType", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("GetType", typeof(DungeonCrawler.Lua.LuaAPI), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_2f05dc39d93c4eae86c1f6c721bece1c()}));
                this.AddMember("ToString", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("ToString", typeof(DungeonCrawler.Lua.LuaAPI), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_07b66c39fd5f4923b416498d46dea395()}));
                this.AddMember("Equals", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("Equals", typeof(DungeonCrawler.Lua.LuaAPI), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_a1bf81f3941f4f019625bbfe28efde2f()}));
                this.AddMember("GetHashCode", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("GetHashCode", typeof(DungeonCrawler.Lua.LuaAPI), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_5b992cee3ba047dfbb5d0efcf6a7230f()}));
            }
            
            private sealed class MTHD_d7a7856314364c83811c7745aff813a3 : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_d7a7856314364c83811c7745aff813a3() {
                    this.Initialize(".ctor", true, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[0], false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return new DungeonCrawler.Lua.LuaAPI();
                }
            }
            
            private sealed class MTHD_1c6491be3ab14863ac6310046e2dc881 : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_1c6491be3ab14863ac6310046e2dc881() {
                    this.Initialize("Sum", false, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[] {
                                new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor("x", typeof(int), false, null, false, false, false),
                                new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor("y", typeof(int), false, null, false, false, false)}, false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return ((DungeonCrawler.Lua.LuaAPI)(obj)).Sum(((int)(pars[0])), ((int)(pars[1])));
                }
            }
            
            private sealed class MTHD_2f05dc39d93c4eae86c1f6c721bece1c : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_2f05dc39d93c4eae86c1f6c721bece1c() {
                    this.Initialize("GetType", false, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[0], false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return ((object)(obj)).GetType();
                }
            }
            
            private sealed class MTHD_07b66c39fd5f4923b416498d46dea395 : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_07b66c39fd5f4923b416498d46dea395() {
                    this.Initialize("ToString", false, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[0], false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return ((object)(obj)).ToString();
                }
            }
            
            private sealed class MTHD_a1bf81f3941f4f019625bbfe28efde2f : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_a1bf81f3941f4f019625bbfe28efde2f() {
                    this.Initialize("Equals", false, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[] {
                                new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor("obj", typeof(object), false, null, false, false, false)}, false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return ((object)(obj)).Equals(((object)(pars[0])));
                }
            }
            
            private sealed class MTHD_5b992cee3ba047dfbb5d0efcf6a7230f : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_5b992cee3ba047dfbb5d0efcf6a7230f() {
                    this.Initialize("GetHashCode", false, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[0], false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return ((object)(obj)).GetHashCode();
                }
            }
        }
        #endregion
        
        #region Descriptor of DungeonCrawler.Lua.LuaContext
        // Descriptor of DungeonCrawler.Lua.LuaContext
        private sealed class TYPE_373c1f0427c24695ac0513bb4ed27118 : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredUserDataDescriptor {
            
            internal TYPE_373c1f0427c24695ac0513bb4ed27118() : 
                    base(typeof(DungeonCrawler.Lua.LuaContext)) {
                this.AddMember("__new", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("__new", typeof(DungeonCrawler.Lua.LuaContext), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_7e5e6b35b1a84132b226d0a82cb1023a()}));
                this.AddMember("SetPlayerPosition", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("SetPlayerPosition", typeof(DungeonCrawler.Lua.LuaContext), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_aacd380da95a4375923a98f67debdae6()}));
                this.AddMember("GetType", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("GetType", typeof(DungeonCrawler.Lua.LuaContext), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_364bdb5eb2d147fabd1919418b46e09b()}));
                this.AddMember("ToString", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("ToString", typeof(DungeonCrawler.Lua.LuaContext), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_385ad9ce8ea7425d9c1254259636be94()}));
                this.AddMember("Equals", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("Equals", typeof(DungeonCrawler.Lua.LuaContext), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_227893cccd1b48cfb1f734de6d02c984()}));
                this.AddMember("GetHashCode", new MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor("GetHashCode", typeof(DungeonCrawler.Lua.LuaContext), new MoonSharp.Interpreter.Interop.BasicDescriptors.IOverloadableMemberDescriptor[] {
                                new MTHD_e9dad44865ad496ea02fa634e457f5e0()}));
            }
            
            private sealed class MTHD_7e5e6b35b1a84132b226d0a82cb1023a : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_7e5e6b35b1a84132b226d0a82cb1023a() {
                    this.Initialize(".ctor", true, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[] {
                                new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor("context", typeof(DungeonCrawler.Lua.IScriptContext), false, null, false, false, false)}, false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return new DungeonCrawler.Lua.LuaContext(((DungeonCrawler.Lua.IScriptContext)(pars[0])));
                }
            }
            
            private sealed class MTHD_aacd380da95a4375923a98f67debdae6 : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_aacd380da95a4375923a98f67debdae6() {
                    this.Initialize("SetPlayerPosition", false, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[] {
                                new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor("x", typeof(int), false, null, false, false, false),
                                new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor("y", typeof(int), false, null, false, false, false)}, false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    ((DungeonCrawler.Lua.LuaContext)(obj)).SetPlayerPosition(((int)(pars[0])), ((int)(pars[1])));
                    return MoonSharp.Interpreter.DynValue.Void;
                }
            }
            
            private sealed class MTHD_364bdb5eb2d147fabd1919418b46e09b : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_364bdb5eb2d147fabd1919418b46e09b() {
                    this.Initialize("GetType", false, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[0], false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return ((object)(obj)).GetType();
                }
            }
            
            private sealed class MTHD_385ad9ce8ea7425d9c1254259636be94 : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_385ad9ce8ea7425d9c1254259636be94() {
                    this.Initialize("ToString", false, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[0], false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return ((object)(obj)).ToString();
                }
            }
            
            private sealed class MTHD_227893cccd1b48cfb1f734de6d02c984 : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_227893cccd1b48cfb1f734de6d02c984() {
                    this.Initialize("Equals", false, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[] {
                                new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor("obj", typeof(object), false, null, false, false, false)}, false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return ((object)(obj)).Equals(((object)(pars[0])));
                }
            }
            
            private sealed class MTHD_e9dad44865ad496ea02fa634e457f5e0 : MoonSharp.Interpreter.Interop.StandardDescriptors.HardwiredDescriptors.HardwiredMethodMemberDescriptor {
                
                internal MTHD_e9dad44865ad496ea02fa634e457f5e0() {
                    this.Initialize("GetHashCode", false, new MoonSharp.Interpreter.Interop.BasicDescriptors.ParameterDescriptor[0], false);
                }
                
                protected override object Invoke(MoonSharp.Interpreter.Script script, object obj, object[] pars, int argscount) {
                    return ((object)(obj)).GetHashCode();
                }
            }
        }
        #endregion
    }
}
