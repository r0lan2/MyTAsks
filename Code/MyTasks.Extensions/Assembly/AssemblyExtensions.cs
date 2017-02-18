//INSTANT C# NOTE: Formerly VB project-level imports:

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace BigLamp.Extensions.Assembly
{
    public static class AssemblyExtensions
    {

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Infers the Default Namespace of the provided assembly
        /// </summary>
        /// <returns>The inferred Default Namespace of the provided assembly
        /// </returns>
        /// <remarks>
        /// Since there is no "Default Namespace" property of an Assembly,
        /// returns the greatest common namespace segment derived from all
        /// the Types and Resources within the assembly. For best accuracy,
        /// there should be at least one type or resource defined in the "default namespace"
        /// Code from: http://www.developersdex.com/gurus/articles/828.asp?Page=1
        /// </remarks>
        public static string InferDefaultNamespace(this System.Reflection.Assembly asm)
        {
//INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
//		Type t = null;
            var ns = string.Empty;
            var lst = new StringDictionary();
            var excludeNamespace = new List<string> { "jetbrains","my"};

            // Get all types in the assembly and add their namespaces to the list
            foreach (var t in asm.GetTypes())
            {
                if (t.Namespace != null && !(lst.ContainsKey(t.Namespace.ToLower())))
                {
                    if (excludeNamespace.All(val => t.Namespace.ToLower().IndexOf(val, System.StringComparison.Ordinal) == -1))
                    {
                        lst.Add(t.Namespace.ToLower(), t.Namespace);
                    }

                }
            }

            // Get all resources in the assembly and add their namespaces to the list
            foreach (string res in asm.GetManifestResourceNames())
            {
                if (!(lst.ContainsKey(res.ToLower())))
                {
                    lst.Add(res.ToLower(), res);
                }
            }


            if (lst.Count == 1)
            {
                // There is only one namespace in the assembly
                var arrValues = new string[1];
                lst.Values.CopyTo(arrValues, 0);
                ns = arrValues[0];
            }
            else if (lst.Count > 1)
            {
                // There are multiple namespaces in the assembly.
                // We must compare them now.

                string strShortestNs = string.Empty;
//INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
//			string nsTemp = null;

                // Get the shortest namespace as a starting point.
                // We are looking for common nodes in the namespaces, so
                // it is only logical to start with the shortest one.
                foreach (string nsTemp in lst.Keys)
                {
                    if (string.IsNullOrEmpty(strShortestNs) && !string.IsNullOrEmpty(nsTemp))
                    {
                        strShortestNs = string.Copy(lst[nsTemp]);
                    }

                    if ((nsTemp == null ? 0 : nsTemp.Length) < (strShortestNs.Length))
                    {
                        if (nsTemp != null) strShortestNs = string.Copy(lst[nsTemp]);
                    }
                }

                int i;
                var nodes = strShortestNs.Split('.');
                var nodeStack = new ArrayList();
                var blnContinue = true;

                // Begin comparing nodes:
                // For each node on the shortest namespace...
                for (i = 0; i < nodes.Length; i++)
                {
                    // Compare it to the corresponding node in the other namespaces.
                    foreach (string nsTemp in lst.Keys)
                    {
                        // There is no point in comparing the shortest namespace to itself.
                        if (strShortestNs.ToLower() != nsTemp)
                        {
                            // Get the nodes of the namespace being compared.
                            string[] nodesTemp = lst[nsTemp].Split('.');
                            if (nodesTemp.Length >= i)
                            {
                                if (nodes[i] != nodesTemp[i])
                                {
                                    // Has a different node. Nodes are no longer common. Exit for.
                                    blnContinue = false;
                                    break;
                                }
                            }
                            else
                            {
                                // Has less nodes. Shouldn't happen, but if it does, exit for.
                                blnContinue = false;
                                break;
                            }
                        }
                    }

                    // If the node was acceptable
                    if (blnContinue)
                    {
                        // Add it to the list of nodes of the common namespace
                        nodeStack.Add(nodes[i]);
                    }
                    else
                    {
                        // Otherwise quit comparing
                        break;
                    }
                }

                // Assemble nodes to form greatest common Namespace.
                ns = string.Join(".", (string[])nodeStack.ToArray(typeof(string)));

            }


            return ns;
        }

        public static System.Reflection.Assembly GetReferencedAssembly(this System.Reflection.Assembly executingAssembly, string assemblyFullName )
        {
            var assemblyName = executingAssembly
                           .GetReferencedAssemblies()
                           .First(x => x.Name == assemblyFullName);

            return System.Reflection.Assembly.Load(assemblyName);
        }

    }
}
