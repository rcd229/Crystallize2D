using UnityEngine;
using System.Collections;

/**
 * IMenuAcceptor represents data used as menu items which are represented in a nested structure
 * For example, an image may contain text in it, have values associated with it, or have another image
 * inside it which again may have children
 * 
 * This nested structure is implemented with hierarchical visitor pattern.
 */ 
public interface IMenuAcceptor {
	bool Accept(MenuVisitor visitor);
}
