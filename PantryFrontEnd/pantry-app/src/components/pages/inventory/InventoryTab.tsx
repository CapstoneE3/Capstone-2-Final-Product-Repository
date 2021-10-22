import { CircularProgress, Container } from "@material-ui/core";
import { Item } from "./mockEntries";
import React from "react";
import InventoryEntry from "./InventoryEntry";

interface InventoryTabProps {
  category?: string;
  activeTab: number;
  propEntries: Item[];
  index: number;
}

const InventoryTab: React.FC<InventoryTabProps> = ({
  activeTab,
  propEntries,
  index,
}) => {
  window.scrollTo(0, 0);
  return (
    <>
      {activeTab === index && (
        <Container
          style={{
            maxWidth: "none",
            textAlign: "center",
            overflow: "hidden",
            marginBottom: "2rem",
          }}
        >
          {propEntries.length > 0 ? (
            propEntries.map((item) => {
              return (
                <InventoryEntry
                  key={item.itemID}
                  name={item.name}
                  inExpiryGroups={item.expiry_Count}
                  quantity={item.quantity}
                  category={item.category}
                  itemID={item.itemID}
                  photo={item.photo}
                />
              );
            })
          ) : (
            <CircularProgress color="primary" size={100} />
          )}
        </Container>
      )}
    </>
  );
};

// const entriesAreSame = (A: Item[], B: Item[]) => {
//   if (A.length !== B.length) return false;
//   for (let i = 0; i < A.length; i++) {
//     if (A[i]?.iid !== B[i]?.iid) return false;
//   }
//   return true;
// };

// const areEqual: (
//   prevProps: Readonly<InventoryTabProps & { children?: ReactNode }>,
//   nextProps: Readonly<InventoryTabProps & { children?: ReactNode }>
// ) => boolean = (prevProps, nextProps) => {
//   if (
//     prevProps.activeTab === nextProps.activeTab &&
//     entriesAreSame(prevProps.propEntries, nextProps.propEntries)
//   ) {
//     return true;
//   }
//   return false;
// };

// const MemoizedInventoryTab = React.memo(InventoryTab, areEqual);
export default InventoryTab;
