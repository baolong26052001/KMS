import React from 'react';
import PropTypes from 'prop-types';
import { format } from 'date-fns';

const DateFormatter = ({ date }) => {
  const formattedDate = format(new Date(date), 'yyyy-MM-dd HH:mm:ss');
  return <span>{formattedDate}</span>;
};

DateFormatter.propTypes = {
  date: PropTypes.string.isRequired,
};

export default DateFormatter;