using UnityEngine;
using System.Collections;


/**
 * This Interface represents any algorithms that will traverse the nested structure of menu item
 * Each algorithm will have to implement all the visit functions.
 */ 
public interface MenuVisitor {

	/**
	 * visit a node of type MenuAcceptor
	 * Uses Hierachical Visitor Pattern
	 */
	//place holders. Don't use them
	bool Visit(MenuNodeLeaf node);
	bool VisitEnter (MenuNodeCompo node);
	bool VisitLeave (MenuNodeCompo node);

	//Composite nodes
	bool VisitEnter (ImageMenuNode node);
	bool VisitLeave (ImageMenuNode node);
	//Leaf nodes
	bool Visit (TextMenuNode node);
	bool Visit (ValuedMenuNode node);
	//TODO add types
}
