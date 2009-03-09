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

namespace QFXParser
{
    public class Element
    {
        string m_name;
        string m_text;
        List<Element> m_children;

        public Element() 
        {
            m_children = new List<Element>();
        }

        public void appendText(string text)
        {
            if (m_text == null)
                m_text = text;
            else
                m_text = m_text + text;
        }

        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }


        public string Text
        {
            get
            {
                return m_text;
            }
            set
            {
                m_text = value;
            }
        }

        public List<Element> Children
        {
            get
            {
                return m_children;
            }
            set
            {
                m_children = value;
            }
        }
    }
}
