import React from "react";
import { LayoutContentWrapper } from "./layoutWrapper.style";
import styled from "styled-components";

export default styled.div`
  <LayoutContentWrapper
    className="isoLayoutContentWrapper"
  >
    {this.props.children}
  </LayoutContentWrapper>
`;