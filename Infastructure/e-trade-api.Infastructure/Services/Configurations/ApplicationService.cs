using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using e_trade_api.application;

namespace e_trade_api.Infrastructure
{
    public class ApplicationService : IApplicationService
    {
        public List<Menu> GetAuthorizeDefinitionEndpoints(Type type)
        {
            Assembly? assembly = Assembly.GetAssembly(type);
            var controllers = assembly
                ?.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<Menu> menus = new List<Menu>();

            if (controllers != null)
            {
                foreach (var controller in controllers)
                {
                    var actions = controller
                        .GetMethods()
                        .Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));

                    if (actions != null)
                    {
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);

                            if (attributes != null)
                            {
                                var authorizeDefinitionAttribute =
                                    attributes.FirstOrDefault(
                                        a => a.GetType() == typeof(AuthorizeDefinitionAttribute)
                                    ) as AuthorizeDefinitionAttribute;

                                if (authorizeDefinitionAttribute != null)
                                {
                                    var menuName = authorizeDefinitionAttribute.Menu;
                                    var actionType = Enum.GetName(
                                        typeof(ActionType),
                                        authorizeDefinitionAttribute.ActionType
                                    );
                                    var definition = authorizeDefinitionAttribute.Definition;

                                    var httpAttribute =
                                        attributes.FirstOrDefault(
                                            a =>
                                                a.GetType()
                                                    .IsAssignableTo(typeof(HttpMethodAttribute))
                                        ) as HttpMethodAttribute;

                                    var httpType =
                                        httpAttribute != null
                                            ? httpAttribute.HttpMethods.First()
                                            : HttpMethods.Get;

                                    var actionCode =
                                        $"{httpType}.{actionType}.{definition.Replace(" ", "")}";

                                    var existingMenu = menus.FirstOrDefault(
                                        m => m.Name == menuName
                                    );

                                    if (existingMenu == null)
                                    {
                                        var newMenu = new Menu
                                        {
                                            Name = menuName,
                                            Actions = new List<application.Action>()
                                        };

                                        var newAction = new application.Action
                                        {
                                            ActionType = actionType,
                                            Definition = definition,
                                            HttpType = httpType,
                                            Code = actionCode
                                        };

                                        newMenu.Actions.Add(newAction);
                                        menus.Add(newMenu);
                                    }
                                    else
                                    {
                                        var newAction = new application.Action
                                        {
                                            ActionType = actionType,
                                            Definition = definition,
                                            HttpType = httpType,
                                            Code = actionCode
                                        };

                                        existingMenu.Actions.Add(newAction);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return menus;
        }
    }
}
