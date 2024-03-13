${
	["MoonSharp.Interpreter.Interop.AnonWrapper"] = ${
		skip = true,
	},
	["MoonSharp.Interpreter.Serialization.Json.JsonNull"] = ${
		skip = true,
	},
	["DungeonCrawler.Lua.LuaAPI"] = ${
		visibility = "public",
		class = "MoonSharp.Interpreter.Interop.StandardUserDataDescriptor",
		members = ${
			__new = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "__new",
				decltype = "DungeonCrawler.Lua.LuaAPI",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = ".ctor",
						ctor = true,
						special = true,
						visibility = "public",
						ret = "DungeonCrawler.Lua.LuaAPI",
						decltype = "DungeonCrawler.Lua.LuaAPI",
						static = true,
						extension = false,
						params = ${ },
					},
				},
			},
			Sum = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "Sum",
				decltype = "DungeonCrawler.Lua.LuaAPI",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = "Sum",
						ctor = false,
						special = false,
						visibility = "public",
						ret = "System.Int32",
						decltype = "DungeonCrawler.Lua.LuaAPI",
						static = false,
						extension = false,
						params = ${
							[1] = ${
								name = "x",
								type = "System.Int32",
								origtype = "System.Int32",
								default = false,
								out = false,
								ref = false,
								varargs = false,
								restricted = false,
							},
							[2] = ${
								name = "y",
								type = "System.Int32",
								origtype = "System.Int32",
								default = false,
								out = false,
								ref = false,
								varargs = false,
								restricted = false,
							},
						},
					},
				},
			},
			GetType = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "GetType",
				decltype = "DungeonCrawler.Lua.LuaAPI",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = "GetType",
						ctor = false,
						special = false,
						visibility = "public",
						ret = "System.Type",
						decltype = "System.Object",
						static = false,
						extension = false,
						params = ${ },
					},
				},
			},
			ToString = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "ToString",
				decltype = "DungeonCrawler.Lua.LuaAPI",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = "ToString",
						ctor = false,
						special = false,
						visibility = "public",
						ret = "System.String",
						decltype = "System.Object",
						static = false,
						extension = false,
						params = ${ },
					},
				},
			},
			Equals = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "Equals",
				decltype = "DungeonCrawler.Lua.LuaAPI",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = "Equals",
						ctor = false,
						special = false,
						visibility = "public",
						ret = "System.Boolean",
						decltype = "System.Object",
						static = false,
						extension = false,
						params = ${
							[1] = ${
								name = "obj",
								type = "System.Object",
								origtype = "System.Object",
								default = false,
								out = false,
								ref = false,
								varargs = false,
								restricted = false,
							},
						},
					},
				},
			},
			GetHashCode = ${
				class = "MoonSharp.Interpreter.Interop.OverloadedMethodMemberDescriptor",
				name = "GetHashCode",
				decltype = "DungeonCrawler.Lua.LuaAPI",
				overloads = ${
					[1] = ${
						class = "MoonSharp.Interpreter.Interop.MethodMemberDescriptor",
						name = "GetHashCode",
						ctor = false,
						special = false,
						visibility = "public",
						ret = "System.Int32",
						decltype = "System.Object",
						static = false,
						extension = false,
						params = ${ },
					},
				},
			},
		},
		metamembers = ${ },
	},
}
