using Cuit.Control;
using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Cuit.Screen
{
    public class DynamicControlsObject : DynamicObject
    {
        private readonly Dictionary<string, IControl> _controlsNameMappings;
        public DynamicControlsObject(Dictionary<string, IControl> controlsNameMappings)
        {
            _controlsNameMappings = controlsNameMappings;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_controlsNameMappings.ContainsKey(binder.Name))
            {
                result = _controlsNameMappings[binder.Name];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }

    public class FormScreenXml : FormScreen, IDynamicMetaObjectProvider
    {
        private readonly Dictionary<string, IControl> _controlsNameMapping;
        private readonly DynamicControlsObject _dynamicControlsObject;

        public FormScreenXml(string xmlPath)
        {
            _controlsNameMapping = FormXmlParser.Parse(xmlPath);
            Controls.AddRange(_controlsNameMapping.Values);

            _dynamicControlsObject = new DynamicControlsObject(_controlsNameMapping);
        }


        /// <summary>
        /// Use with caution
        /// </summary>
        protected dynamic DynamicSelf => (dynamic)this;

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new DelegatingMetaObject(_dynamicControlsObject, parameter, BindingRestrictions.GetTypeRestriction(parameter, this.GetType()), this);
        }

        protected dynamic this[string name]
        {
            get
            {
                if (!_controlsNameMapping.ContainsKey(name))
                    throw new Exception($"No such control {name}");

                return _controlsNameMapping[name];
            }
        }

        private class DelegatingMetaObject : DynamicMetaObject
        {
            private readonly IDynamicMetaObjectProvider innerProvider;

            public DelegatingMetaObject(IDynamicMetaObjectProvider innerProvider, Expression expr, BindingRestrictions restrictions)
                : base(expr, restrictions)
            {
                this.innerProvider = innerProvider;
            }

            public DelegatingMetaObject(IDynamicMetaObjectProvider innerProvider, Expression expr, BindingRestrictions restrictions, object value)
                : base(expr, restrictions, value)
            {
                this.innerProvider = innerProvider;
            }

            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                var innerMetaObject = innerProvider.GetMetaObject(Expression.Constant(innerProvider));
                return innerMetaObject.BindGetMember(binder);
            }
        }
    }
}
