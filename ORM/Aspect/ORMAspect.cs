﻿/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Reflection.AOP.Interfaces;
using Utilities.ORM.Mapping.Interfaces;
using Utilities.Reflection.Emit.Interfaces;
using Utilities.ORM.Aspect.Interfaces;
using System.Reflection;
using Utilities.Reflection.Emit.BaseClasses;
using Utilities.Reflection.Emit.Enums;
using System.Reflection.Emit;
#endregion

namespace Utilities.ORM.Aspect
{
    /// <summary>
    /// ORM Aspect (used internally)
    /// </summary>
    public class ORMAspect : IAspect
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ORMAspect(Dictionary<Type, IMapping> Mappings)
        {
            this.InterfacesUsing = new List<Type>();
            this.InterfacesUsing.Add(typeof(IORMObject));
            ClassMappings = Mappings;
        }

        #endregion

        #region Functions

        public void SetupStartMethod(Reflection.Emit.Interfaces.IMethodBuilder Method, Type BaseType)
        {
            Method.SetCurrentMethod();
            if (ClassMappings.ContainsKey(BaseType)
                && Method.Name.StartsWith("set_"))
            {
                IMapping Mapping = ClassMappings[BaseType];
                string PropertyName = Method.Name.Replace("set_", "");
                IProperty Property = Mapping.Properties.Find(x => x.Name == PropertyName);
                if (Property != null)
                {
                    Utilities.Reflection.Emit.FieldBuilder Field = Fields.Find(x => x.Name == Property.DerivedFieldName);
                    if (Field != null)
                        Field.Assign(Method.Parameters[1]);
                }
            }
        }

        public void SetupEndMethod(Reflection.Emit.Interfaces.IMethodBuilder Method, Type BaseType, Reflection.Emit.BaseClasses.VariableBase ReturnValue)
        {
            if (ClassMappings.ContainsKey(BaseType)
                && Method.Name.StartsWith("get_"))
            {
                IMapping Mapping = ClassMappings[BaseType];
                string PropertyName = Method.Name.Replace("set_", "");
                IProperty Property = Mapping.Properties.Find(x => x.Name == PropertyName);
                if (Property != null)
                {
                    if(Property is IManyToOne)
                        SetupSingleProperty(Method, BaseType, ReturnValue, Property);
                    else if(Property is IMap)
                        SetupSingleProperty(Method, BaseType, ReturnValue, Property);
                    else if(Property is IIEnumerableManyToOne)
                        SetupIEnumerableProperty(Method, BaseType, ReturnValue, Property);
                    else if(Property is IManyToMany)
                        SetupIEnumerableProperty(Method, BaseType, ReturnValue, Property);
                }
            }
        }

        public void SetupExceptionMethod(Reflection.Emit.Interfaces.IMethodBuilder Method, Type BaseType)
        {
            
        }

        public void Setup(object Object)
        {
            
        }

        public void SetupInterfaces(Reflection.Emit.TypeBuilder TypeBuilder)
        {
            Fields = new List<Reflection.Emit.FieldBuilder>();
            SessionField = CreateProperty(TypeBuilder, "Session0", typeof(Session));
            CreateConstructor(TypeBuilder);
        }

        /// <summary>
        /// Creates the default constructor
        /// </summary>
        /// <param name="TypeBuilder">Type builder</param>
        private void CreateConstructor(Reflection.Emit.TypeBuilder TypeBuilder)
        {
            IMethodBuilder Constructor = TypeBuilder.CreateConstructor();
            {
                Constructor.SetCurrentMethod();
                Constructor.This.Call(TypeBuilder.BaseClass.GetConstructor(Type.EmptyTypes));
                Constructor.Return();
            }
        }

        /// <summary>
        /// Creates a default property
        /// </summary>
        /// <param name="TypeBuilder">Type builder</param>
        /// <param name="Name">Name of the property</param>
        /// <param name="PropertyType">Property type</param>
        /// <returns>The property builder</returns>
        private IPropertyBuilder CreateProperty(Reflection.Emit.TypeBuilder TypeBuilder, string Name, Type PropertyType)
        {
            return TypeBuilder.CreateDefaultProperty(Name, PropertyType, PropertyAttributes.SpecialName,
                MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Public,
                MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Public);
        }

        /// <summary>
        /// Sets up a property (IEnumerable)
        /// </summary>
        /// <param name="Method">Method builder</param>
        /// <param name="BaseType">Base type for the object</param>
        /// <param name="ReturnValue">Return value</param>
        /// <param name="Property">Property info</param>
        private void SetupIEnumerableProperty(IMethodBuilder Method, Type BaseType, Reflection.Emit.BaseClasses.VariableBase ReturnValue, IProperty Property)
        {
            Utilities.Reflection.Emit.FieldBuilder Field = Fields.Find(x => x.Name == Property.DerivedFieldName);
            Utilities.Reflection.Emit.Commands.If If1 = Method.If((VariableBase)SessionField, Comparison.NotEqual, null);
            {
                Utilities.Reflection.Emit.Commands.If If2 = Method.If(Field, Comparison.Equal, null);
                {
                    //Load data

                    //VariableBase FieldType = Method.CreateLocal(Field.Name + "FieldType", Field.DataType);
                    //VariableBase Parameter = Method.CreateLocal(Field.Name + "FieldType", Field.DataType);
                    //Method.Generator.Emit(OpCodes.Ldarg_0);
                    //Method.Generator.EmitCall(OpCodes.Call, SessionField.GetMethod.Builder, Type.EmptyTypes);
                    //Method.Generator.Emit(OpCodes.Ldarg_0);
                    //Method.Generator.Emit(OpCodes.Ldfld, IDField.Builder);
                    //Method.Generator.Emit(OpCodes.Box, IDField.DataType);
                    //Method.Generator.Emit(OpCodes.Ldarg_0);
                    //Method.Generator.Emit(OpCodes.Ldflda, Field.Builder);
                    //Method.Generator.Emit(OpCodes.Ldstr, Attribute.Name);
                    //MethodInfo TempMethod = typeof(HaterAide.Session).GetMethod("SelectList");
                    //TempMethod = TempMethod.MakeGenericMethod(new Type[] { BaseType, Field.DataType.GetGenericArguments()[0] });
                    //Method.Generator.Emit(OpCodes.Callvirt, TempMethod);
                }
                If2.EndIf();
                Utilities.Reflection.Emit.Commands.If If3 = Method.If(Field, Comparison.Equal, null);
                {
                    Field.Assign(Method.NewObj(Field.DataType.GetConstructor(Type.EmptyTypes)));
                }
                If3.EndIf();
            }
            If1.EndIf();
            ReturnValue.Assign(Field);
        }

        /// <summary>
        /// Sets up a property (non IEnumerable)
        /// </summary>
        /// <param name="Method">Method builder</param>
        /// <param name="BaseType">Base type for the object</param>
        /// <param name="ReturnValue">Return value</param>
        /// <param name="Property">Property info</param>
        private void SetupSingleProperty(IMethodBuilder Method, Type BaseType, Reflection.Emit.BaseClasses.VariableBase ReturnValue, IProperty Property)
        {
            Utilities.Reflection.Emit.FieldBuilder Field = Fields.Find(x => x.Name == Property.DerivedFieldName);
            Utilities.Reflection.Emit.Commands.If If1 = Method.If((VariableBase)SessionField, Comparison.NotEqual, null);
            {
                Utilities.Reflection.Emit.Commands.If If2 = Method.If(Field, Comparison.Equal, null);
                {
                    //Load Data

                    //VariableBase FieldType = Method.CreateLocal(Field.Name + "FieldType", Field.DataType);
                    //Method.Generator.Emit(OpCodes.Ldarg_0);
                    //Method.Generator.EmitCall(OpCodes.Call, SessionField.GetMethod.Builder, Type.EmptyTypes);
                    //Method.Generator.Emit(OpCodes.Ldarg_0);
                    //Method.Generator.Emit(OpCodes.Ldfld, IDField.Builder);
                    //Method.Generator.Emit(OpCodes.Box, IDField.DataType);
                    //Method.Generator.Emit(OpCodes.Ldarg_0);
                    //Method.Generator.Emit(OpCodes.Ldflda, Field.Builder);
                    //MethodInfo TempMethod = typeof(HaterAide.Session).GetMethod("Select");
                    //TempMethod = TempMethod.MakeGenericMethod(new Type[] { Field.DataType });
                    //Method.Generator.EmitCall(OpCodes.Call, TempMethod, Type.EmptyTypes);
                }
                If2.EndIf();
            }
            If1.EndIf();
            ReturnValue.Assign(Field);
        }

        #endregion

        #region Properties

        public virtual List<Type> InterfacesUsing { get; set; }

        /// <summary>
        /// Class mappings
        /// </summary>
        private Dictionary<Type, IMapping> ClassMappings { get; set; }

        /// <summary>
        /// Fields used for storing Map, ManyToOne, and ManyToMany properties
        /// </summary>
        private List<Utilities.Reflection.Emit.FieldBuilder> Fields { get; set; }

        /// <summary>
        /// Field to store the session object
        /// </summary>
        private IPropertyBuilder SessionField { get; set; }

        #endregion
    }
}