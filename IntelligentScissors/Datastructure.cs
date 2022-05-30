using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
	public class Datastructure
	{

        public Datastructure()
        {

        }

        private List<edge> tree = new List<edge>();

        private int parent_edge(int edge)
        {
            int parent = (edge - 1) / 2;
            return parent;
        }
        
        private int left_edge(int edge)
        {
            int left = edge * 2 + 1;
            return left;
            
        }
        private int right_edge(int edge)
        {
            int right = edge * 2 + 2;
            return right;
        }

        private void swapedges(edge I, edge J)
        {
            edge tmp = I;
            I = J;
            J = tmp;
        }


        private void repositionup(int edge)
        {
            if (tree[edge].weight >= tree[parent_edge(edge)].weight)
                return;
            if (edge == 0)
                return;
            else
            {
                edge temp = tree[parent_edge(edge)];
                tree[parent_edge(edge)] = tree[edge];
                tree[edge] = temp;
                repositionup(parent_edge(edge));
            }
        }
       
        private void repositiondown(int edge)
        {
            if (right_edge(edge) >= tree.Count)
            {
                if (left_edge(edge) < tree.Count) {
                    if (tree[left_edge(edge)].weight >= tree[edge].weight)
                              return;
                }
            }

            if (left_edge(edge) < tree.Count && tree[left_edge(edge)].weight >= tree[edge].weight )
            {
                if (right_edge(edge) < tree.Count && tree[right_edge(edge)].weight >= tree[edge].weight)
                {
                    return;
                }
            }
            if (left_edge(edge) >= tree.Count)
                     return;
            if (right_edge(edge) < tree.Count && tree[right_edge(edge)].weight <= tree[left_edge(edge)].weight)
            {
                edge temp = tree[right_edge(edge)];
                tree[right_edge(edge)] = tree[edge];
                tree[edge] = temp;
                repositiondown(right_edge(edge));
            }

            else
            {
                edge temp = tree[left_edge(edge)];
                tree[left_edge(edge)] = tree[edge];
                tree[edge] = temp;
                repositiondown(left_edge(edge));
            }
        }

        public void Push(edge edge)
        {
            tree.Add(edge);
            repositionup(tree.Count - 1);
        }

        public edge Pop()
        {
            edge temp = tree[0];
            tree[0] = tree[tree.Count - 1];
            tree.RemoveAt(tree.Count - 1);
            repositiondown(0);
            return temp;
        }

        public bool empty()
        {
            if (tree.Count == 0)
                return true;
            return false;
        }

        public edge Top()
        {

            return tree[0];
        }


    }
}
