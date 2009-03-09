/*
  QFX Parser Library - low level utility to parse Quicken QFX files into a basic
  object model.
 
  Copyright (C) 2009  Stephen P Owens

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QFXParser
{
    public class Parser
    {
        Stack<Token> m_tokStack;
        Stack<Element> m_eltStack;

        Scanner m_scanner;
        bool m_finished = false;
        public Parser(Stream input, int tabStop)
        {
            m_scanner = new Scanner(input, tabStop);
            //m_tokStack = new Stack<Token>();
            m_eltStack = new Stack<Element>();
        }

        void procToken(Token t)
        {
            switch (t.Type)
            {
                case Token.TokType.OPEN_TAG:
                    Element e = new Element();
                    e.Name = t.Value;
                    //m_tokStack.Push(t);
                    m_eltStack.Push(e);
                    break;
                case Token.TokType.CLOSE_TAG:
                    List<Element> lst = new List<Element>();
                    Element top = m_eltStack.Peek();
                    string name = top.Name;
                    while (name.CompareTo(t.Value) != 0)
                    {
                        lst.Add(m_eltStack.Pop());
                        top = m_eltStack.Peek();
                        name = top.Name;
                    }
                    top.Children.AddRange(lst);
                    if(name.CompareTo("OFX") == 0)
                        m_finished = true;
                    break;
                case Token.TokType.TEXT:
                    m_eltStack.Peek().appendText(t.Value);
                    break;
                case Token.TokType.EOF:
                    throw new ParserException(t, "Unexpected end of file");                    
            }
        }
        public Element parse()
        {
            m_finished = false;
            Token t = m_scanner.getNextToken();
            while ((t.Type != Token.TokType.EOF) && (! m_finished))
            {
                procToken(t);
                t = m_scanner.getNextToken();
            }
            if (m_finished)
                return m_eltStack.Pop();
            return null;
        }
    }
}
