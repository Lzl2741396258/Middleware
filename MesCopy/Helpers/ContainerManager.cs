using Autofac;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Autofac.IContainer;

namespace Helpers
{
    public class ContainerManager
    {
        private static readonly object _autofacLock = new object();
        private static readonly string DefaultName = "Default";

        private Dictionary<string, IContainer> _containers;
        private Dictionary<string, Lazy<ContainerBuilder>> _containerBuilers;

        private static readonly Lazy<ContainerManager> _instance = new Lazy<ContainerManager>(() => new ContainerManager());
        public static ContainerManager Instance => _instance.Value;

        private ContainerManager()
        {
            _containers = new Dictionary<string, IContainer>();
            _containerBuilers = new Dictionary<string, Lazy<ContainerBuilder>>
            {
                { DefaultName, new Lazy<ContainerBuilder>() }
            };
        }

        // 默认容器
        public IContainer Default
        {
            get
            {
                return GetContainer(DefaultName);
            }
        }

        // 获取指定名称的容器
        public IContainer GetContainer(string name)
        {
            if (!_containers.ContainsKey(name))
            {
                lock (_autofacLock)
                {
                    if (!_containers.ContainsKey(name))
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            return _containers[name];
        }

        // 注册服务到默认容器
        public void Register(Action<ContainerBuilder> action)
        {
            action(_containerBuilers[DefaultName].Value);
        }

        // 注册服务到指定容器
        public void Register(string containerName, Action<ContainerBuilder> action)
        {
            if (!_containerBuilers.ContainsKey(containerName))
            {
                lock (_autofacLock)
                {
                    _containerBuilers.Add(containerName, new Lazy<ContainerBuilder>());
                }
            }

            action(_containerBuilers[containerName].Value);
        }

        // 构建默认容器
        public void Build()
        {
            lock (_autofacLock)
            {
                _containers[DefaultName] = _containerBuilers[DefaultName].Value.Build();
            }
        }

        // 构建指定容器
        public void Build(string containerName)
        {
            lock (_autofacLock)
            {
                if (_containerBuilers.TryGetValue(containerName, out Lazy<ContainerBuilder> value))
                {
                    _containers[containerName] = value.Value.Build();
                }
            }
        }

        // 索引器 - 获取指定容器或服务
        public IContainer this[string containerName]
        {
            get
            {
                return GetContainer(containerName);
            }
        }
    }

}